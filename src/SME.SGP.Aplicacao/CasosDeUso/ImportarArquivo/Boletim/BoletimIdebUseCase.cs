using MediatR;
using Microsoft.AspNetCore.Http;
using SME.SGP.Aplicacao.Commands.ImportarArquivo;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.ImportarArquivo.Boletim;
using SME.SGP.Aplicacao.Queries.ProficienciaIdeb;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.ImportarArquivo;
using SME.SGP.Infra.Enumerados;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.ImportarArquivo.Boletim
{
    public class BoletimIdebUseCase : ImportacaoArquivoBaseUseCase, IBoletimIdebUseCase
    {
        public BoletimIdebUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ImportacaoLogRetornoDto> Executar(int anoLetivo, IEnumerable<IFormFile> boletins)
        {
            if (anoLetivo == 0)
                throw new NegocioException("Informe o ano letivo.");

            if (boletins == null || boletins.Count() == 0)
                throw new NegocioException("Informe o arquivo .pdf");

            var importacaoLog = await SalvarImportacao(TipoArquivoImportacao.BOLETIM_IDEB.GetAttribute<DisplayAttribute>().Name);

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
            ProcessadosComFalha = new List<SalvarImportacaoLogErroDto>();
            int totalRegistros = 0;
            var loteSalvar = new List<Task>();

            try
            {
                var proficienciaIdebs = await mediator.Send(new ObterProficienciaIdebPorAnoLetivoQuery(anoLetivo, boletins.Select(x => x.Name)?.ToList()));

                if (proficienciaIdebs != null && proficienciaIdebs.Any())
                {
                    foreach (var boletim in boletins)
                    {
                        var proficiencia = proficienciaIdebs?.Where(p => p.CodigoEOLEscola == boletim.Name)?.FirstOrDefault();
                        if (proficiencia == null)
                        {
                            SalvarErroLinha(importacaoLogDto.Id, 0, $"Não existe proficiência cadastrada para o ano letivo {anoLetivo} com o nome do arquivo {boletim.Name}.");
                            continue;
                        }

                        var enderecoArquivo = await mediator.Send(new ArmazenarArquivoFisicoCommand(boletim, boletim.Name, TipoArquivo.Importacao));

                        if (string.IsNullOrEmpty(enderecoArquivo))
                        {
                            var proficienciaDto = new ProficienciaIdebDto();
                                 
                            proficienciaDto.Id = proficiencia.Id;
                            proficienciaDto.CodigoEOLEscola = proficiencia.CodigoEOLEscola;
                            proficienciaDto.AnoLetivo = proficiencia.AnoLetivo;

                            //await mediator.Send(new SalvarImportacaoProficienciaIdepCommand(proficienciaDto));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SalvarErroLinha(importacaoLogDto.Id, 0, $"Erro geral na importação: {ex.Message}");
            }
            finally
            {
                importacaoLogDto.TotalRegistros = totalRegistros;
                importacaoLogDto.RegistrosProcessados = totalRegistros - ProcessadosComFalha.Count;
                importacaoLogDto.RegistrosComFalha = ProcessadosComFalha.Count;
                importacaoLogDto.StatusImportacao = ProcessadosComFalha.Count > 0
                ? SituacaoArquivoImportacao.ProcessadoComFalhas.GetAttribute<DisplayAttribute>().Name
                : SituacaoArquivoImportacao.ProcessadoComSucesso.GetAttribute<DisplayAttribute>().Name;
                importacaoLogDto.DataFimProcessamento = DateTime.Now;

                await mediator.Send(new SalvarImportacaoLogCommand(importacaoLogDto));
            }
            return true;
        }
    }
}
