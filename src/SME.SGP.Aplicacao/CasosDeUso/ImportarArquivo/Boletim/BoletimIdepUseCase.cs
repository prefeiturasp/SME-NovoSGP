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
                await ProcessarArquivoAsync(boletins, importacaoLogDto, anoLetivo);
            }
            return ImportacaoLogRetornoDto.RetornarSucesso(MensagemNegocioComuns.ARQUIVO_IMPORTADO_COM_SUCESSO, importacaoLog.Id);
        }

        private async Task<bool> ProcessarArquivoAsync(IEnumerable<IFormFile> boletins, ImportacaoLogDto importacaoLogDto, int anoLetivo)
        {
            var listaLote = new List<ArquivoFluenciaLeitoraDto>();
            processadosComFalha = new List<SalvarImportacaoLogErroDto>();
            int totalRegistros = 0;
            var loteSalvar = new List<Task>();

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
                {
                    proficienciaIdeps = await mediator.Send(new ObterProficienciaIdepPorAnoLetivoQuery(anoLetivo, codigosUesValidos));
                }

                if (proficienciaIdeps != null && proficienciaIdeps.Any())
                {
                    foreach (var boletim in boletins)
                    {
                        totalRegistros++;

                        var nomeArquivo = Path.GetFileNameWithoutExtension(boletim.FileName);

                        if (codigosUesInvalidos.Contains(nomeArquivo))
                        {
                            SalvarErroLinha(importacaoLogDto.Id, processadosComFalha.Count + 1,
                             $"Código de UE '{nomeArquivo}' não é válido ou não foi encontrado no sistema.");
                            continue;
                        }

                        var proficiencia = proficienciaIdeps?.Where(p => p.CodigoEOLEscola == nomeArquivo)?.FirstOrDefault();
                        if (proficiencia == null)
                        {
                            SalvarErroLinha(importacaoLogDto.Id, processadosComFalha.Count + 1, $"Não existe proficiência cadastrada para o ano letivo {anoLetivo} com o nome do arquivo {boletim.Name}.");
                            continue;
                        }

                        var nomeFisico = $"{Guid.NewGuid()}-{nomeArquivo}";
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
            }
            catch (Exception ex)
            {
                SalvarErroLinha(importacaoLogDto.Id, processadosComFalha.Count + 1, $"Erro geral na importação: {ex.Message}");
            }
            finally
            {
                await SalvarImportacaoLog(importacaoLogDto, totalRegistros);
            }
            return true;
        }
    }
}
