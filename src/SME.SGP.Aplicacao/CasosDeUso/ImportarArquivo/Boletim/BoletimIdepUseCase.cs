using MediatR;
using Microsoft.AspNetCore.Http;
using SME.SGP.Aplicacao.Commands.ImportarArquivo.ProficienciaIdep;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.ImportarArquivo.Boletim;
using SME.SGP.Aplicacao.Queries.ProficienciaIdep;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.ImportarArquivo;
using SME.SGP.Infra.Enumerados;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.ImportarArquivo.Boletim
{
    public class BoletimIdepUseCase : ImportacaoArquivoBaseUseCase, IBoletimIdepUseCase
    {

        public BoletimIdepUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ImportacaoLogRetornoDto> Executar(int anoLetivo, IEnumerable<IFormFile> boletins)
        {
            if (anoLetivo == 0)
                throw new NegocioException("Informe o ano letivo.");

            if (boletins == null || boletins.Count() == 0)
                throw new NegocioException("Informe o arquivo .pdf");

            var importacaoLog = await SalvarImportacao(TipoArquivoImportacao.BOLETIM_IDEP.GetAttribute<DisplayAttribute>().Name,
                                                       TipoArquivoImportacao.BOLETIM_IDEP.GetAttribute<DisplayAttribute>().Name);

            if (importacaoLog != null)
            {
                var importacaoLogDto = MapearParaDto(importacaoLog);
                var sucesso = await ProcessarArquivoAsync(boletins, importacaoLogDto, anoLetivo);
                return sucesso
                        ? ImportacaoLogRetornoDto.RetornarSucesso(MensagemNegocioComuns.ARQUIVO_IMPORTADO_COM_SUCESSO)
                        : ImportacaoLogRetornoDto.RetornarFalha("Falha ao processar importação de boletins.");
            }

            return ImportacaoLogRetornoDto.RetornarFalha("Falha ao iniciar importação de boletins.");
        }

        private async Task<bool> ProcessarArquivoAsync(IEnumerable<IFormFile> boletins, ImportacaoLogDto importacaoLogDto, int anoLetivo)
        {
            int totalRegistros = 0;

            try
            {
                var codigoUes = boletins
                                        .Select(x => Path.GetFileNameWithoutExtension(x.FileName))
                                        .ToList();

                var uesEncontradas = await mediator.Send(new ObterUesComDrePorCodigoUesQuery(codigoUes.ToArray()));
                var codigosUesValidos = uesEncontradas.Select(u => u.CodigoUe).ToList();
                var codigosUesInvalidos = codigoUes.Where(codigo => !codigosUesValidos.Contains(codigo)).ToList();

                IEnumerable<ProficienciaIdep> proficienciaIdeps = null;
                if (codigosUesValidos.Any())
                    proficienciaIdeps = await mediator.Send(new ObterProficienciaIdepPorAnoLetivoQuery(anoLetivo, codigosUesValidos));

                foreach (var boletim in boletins)
                {
                    totalRegistros++;

                    var codigoUe = Path.GetFileNameWithoutExtension(boletim.FileName);

                    if (codigosUesInvalidos.Contains(codigoUe))
                    {
                        SalvarErroLinha(importacaoLogDto.Id, processadosComFalha.Count + 1,
                         $"Código de UE '{codigoUe}' não é válido ou não foi encontrado no sistema.");
                        continue;
                    }

                    var proficiencia = proficienciaIdeps?.Where(p => p.CodigoEOLEscola == codigoUe)?.FirstOrDefault();
                    if (proficiencia == null)
                    {
                        SalvarErroLinha(importacaoLogDto.Id, processadosComFalha.Count + 1, $"Não existe proficiência cadastrada para o ano letivo {anoLetivo} com o nome do arquivo '{boletim.FileName}'.");
                        continue;
                    }

                    var nomeFisico = $"{Guid.NewGuid()}-{codigoUe}";
                    var enderecoArquivo = await mediator.Send(new ArmazenarArquivoFisicoCommand(boletim, nomeFisico, TipoArquivo.Importacao));

                    if (!string.IsNullOrEmpty(enderecoArquivo))
                    {
                        var proficienciaDto = new ProficienciaIdepDto(
                             proficiencia.SerieAno,
                             proficiencia.CodigoEOLEscola,
                             proficiencia.AnoLetivo,
                             proficiencia.ComponenteCurricular,
                             proficiencia.Proficiencia,
                             enderecoArquivo
                         );

                        proficienciaDto.Id = proficiencia.Id;

                        await mediator.Send(new SalvarImportacaoProficienciaIdepCommand(proficienciaDto));
                    }
                }
            }
            catch (Exception ex)
            {
                SalvarErroLinha(importacaoLogDto.Id, processadosComFalha.Count + 1, $"Erro geral na importação: {ex.Message}");
            }
            finally
            {
                await SalvarImportacaoLog(importacaoLogDto, totalRegistros);
            }

            return processadosComFalha.Count == 0;
        }
    }
}
