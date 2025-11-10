using ClosedXML.Excel;
using MediatR;
using Microsoft.AspNetCore.Http;
using SME.SGP.Aplicacao.Commands.ImportarArquivo.Alfabetizacao;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.ImportarArquivo;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.ImportarArquivo;
using SME.SGP.Infra.Enumerados;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace SME.SGP.Aplicacao.CasosDeUso.ImportarArquivo
{
    public class ImportacaoArquivoAlfabetizacaoUseCase : ImportacaoArquivoBaseUseCase, IImportacaoArquivoAlfabetizacaoUseCase
    {
        private readonly IRepositorioUeConsulta repositorioUeConsulta;
        public ImportacaoArquivoAlfabetizacaoUseCase(IMediator mediator, IRepositorioUeConsulta repositorioUeConsulta) : base(mediator)
        {
            this.repositorioUeConsulta = repositorioUeConsulta ?? throw new ArgumentNullException(nameof(repositorioUeConsulta));
        }

        public async Task<ImportacaoLogRetornoDto> Executar(IFormFile arquivo, int anoLetivo)
        {
            if (anoLetivo == 0)
                throw new NegocioException("Informe o ano letivo.");

            if (arquivo == null || arquivo.Length == 0)
                throw new NegocioException(MensagemNegocioComuns.ARQUIVO_VAZIO);

            var tipoArquivo = TipoArquivoImportacao.ALFABETIZACAO.GetAttribute<DisplayAttribute>().Name;
            var importacaoLog = await SalvarImportacao(arquivo.Name, tipoArquivo);

            if (importacaoLog != null)
            {
                var importacaoLogDto = MapearParaDto(importacaoLog);
                await ProcessarArquivoAsync(arquivo.OpenReadStream(), importacaoLogDto, anoLetivo);

                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpPainelEducacional.ConsolidarTaxaAlfabetizacaoPainelEducacional, Guid.NewGuid(), null));
            }
            return ImportacaoLogRetornoDto.RetornarSucesso(MensagemNegocioComuns.ARQUIVO_IMPORTADO_COM_SUCESSO, importacaoLog.Id);
        }

        private async Task<bool> ProcessarArquivoAsync(Stream arquivo, ImportacaoLogDto importacaoLogDto, int anoLetivo)
        {
            var listaLote = new List<ArquivoAlfabetizacaoDto>();
            processadosComFalha = new List<SalvarImportacaoLogErroDto>();
            int totalRegistros = 0;
            var loteSalvar = new List<Task>();

            try
            {
                using var package = new XLWorkbook(arquivo);
                var planilha = package.Worksheets.FirstOrDefault();
                if (planilha == null) return false;

                var totalColunas = planilha.Row(1).LastCellUsed().Address.ColumnNumber;
                if (totalColunas < 2)
                    throw new NegocioException(MensagemNegocioComuns.ARQUIVO_NUMERO_COLUNAS_INVALIDO);

                var totalLinhas = planilha.LastRowUsed().RowNumber();
                totalRegistros = totalLinhas - 1;
                var tamanhoLote = 10;

                if (totalLinhas <= 1)
                    throw new NegocioException(MensagemNegocioComuns.ARQUIVO_VAZIO);

                for (int linha = 2; linha <= totalLinhas; linha++)
                {
                    try
                    {
                        var codigoEOLEscola = planilha.Cell(linha, 1).Value.ToString().Trim();
                        decimal.TryParse(planilha.Cell(linha, 2).Value.ToString().Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out var taxaAlfabetizacao);

                        var dto = new ArquivoAlfabetizacaoDto(codigoEOLEscola, taxaAlfabetizacao, anoLetivo)
                        {
                            LinhaAtual = linha
                        };

                        listaLote.Add(dto);

                        if (listaLote.Count == tamanhoLote)
                        {
                            loteSalvar.AddRange(SalvarArquivoAlfabetizacaoEmLote(listaLote, importacaoLogDto.Id));
                            listaLote.Clear();
                        }
                    }
                    catch (Exception ex)
                    {
                        SalvarErroLinha(importacaoLogDto.Id, linha, ex.Message);
                    }
                }

                if (listaLote.Any())
                    loteSalvar.AddRange(SalvarArquivoAlfabetizacaoEmLote(listaLote, importacaoLogDto.Id));

                await Task.WhenAll(loteSalvar);
            }
            catch (Exception ex)
            {
                SalvarErroLinha(importacaoLogDto.Id, 0, $"Erro geral no arquivo: {ex.Message}");
            }
            finally
            {
                await SalvarImportacaoLog(importacaoLogDto, totalRegistros);
            }
            return true;
        }

        private IEnumerable<Task> SalvarArquivoAlfabetizacaoEmLote(List<ArquivoAlfabetizacaoDto> lista, long importacaoLogId)
        {
            foreach (var dto in lista)
            {
                try
                {
                    if (string.IsNullOrEmpty(dto.CodigoEOLEscola))
                    {
                        SalvarErroLinha(importacaoLogId, dto.LinhaAtual, "Código EOL da UE inválido");
                        continue;
                    }

                    if (dto.TaxaAlfabetizacao <= 0 || dto.TaxaAlfabetizacao > 100)
                    {
                        SalvarErroLinha(importacaoLogId, dto.LinhaAtual, "Taxa de alfabetização inválida");
                        continue;
                    }

                    var ue = repositorioUeConsulta.ObterPorCodigo(dto.CodigoEOLEscola);
                    if (ue.EhNulo())
                    {
                        SalvarErroLinha(importacaoLogId, dto.LinhaAtual, "Código EOL da UE não encontrado");
                        continue;
                    }

                    mediator.Send(new SalvarImportacaoArquivoAlfabetizacaoCommand(dto)).GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    SalvarErroLinha(importacaoLogId, dto.LinhaAtual, ex.Message);
                }
            }

            return Enumerable.Empty<Task>();
        }
    }
}
