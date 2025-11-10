using ClosedXML.Excel;
using MediatR;
using Microsoft.AspNetCore.Http;
using SME.SGP.Aplicacao.Commands.ImportarArquivo.Ideb;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SolicitarConsolidacaoProficienciaIdeb;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.ImportarArquivo.Ideb;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Enumerados;
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


namespace SME.SGP.Aplicacao.CasosDeUso.ImportarArquivo.Ideb
{
    public class ImportacaoArquivoIdebUseCase : ImportacaoArquivoBaseUseCase, IImportacaoArquivoIdebUseCase
    {
        private readonly IRepositorioUeConsulta repositorioUeConsulta;

        public ImportacaoArquivoIdebUseCase(IMediator mediator, IRepositorioUeConsulta repositorioUeConsulta) : base(mediator)
        {
            this.repositorioUeConsulta = repositorioUeConsulta ?? throw new ArgumentNullException(nameof(repositorioUeConsulta));
        }

        public async Task<ImportacaoLogRetornoDto> Executar(IFormFile arquivo, int anoLetivo)
        {
            if (anoLetivo == 0)
                throw new NegocioException("Informe o ano letivo.");

            if (arquivo == null || arquivo.Length == 0)
                throw new NegocioException(MensagemNegocioComuns.ARQUIVO_VAZIO);

            if (arquivo.ContentType.NaoEhArquivoXlsx())
                throw new NegocioException(MensagemNegocioComuns.SOMENTE_ARQUIVO_XLSX_SUPORTADO);

            var tipoArquivo = TipoArquivoImportacao.IDEB.GetAttribute<DisplayAttribute>().Name;
            var importacaoLog = await SalvarImportacao(arquivo, tipoArquivo);

            if (importacaoLog != null)
            {
                var importacaoLogDto = MapearParaDto(importacaoLog);
                bool processado = await ProcessarArquivoAsync(arquivo.OpenReadStream(), importacaoLogDto, anoLetivo);
                if (processado)
                {
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ConsolidarPainelEdcacionalIdeb, importacaoLog.Id));
                    await mediator.Send(new SolicitarConsolidacaoProficienciaIdebCommand(anoLetivo));
                }
            }
            return ImportacaoLogRetornoDto.RetornarSucesso(MensagemNegocioComuns.ARQUIVO_IMPORTADO_COM_SUCESSO, importacaoLog.Id);
        }

        private async Task<bool> ProcessarArquivoAsync(Stream arquivo, ImportacaoLogDto importacaoLogDto, int anoLetivo)
        {
            var listaLote = new List<ArquivoIdebDto>();
            processadosComFalha = new List<SalvarImportacaoLogErroDto>();
            int totalRegistros = 0;
            var loteSalvar = new List<Task>();

            try
            {
                using var package = new XLWorkbook(arquivo);
                var planilha = package.Worksheets.FirstOrDefault();
                if (planilha == null) return false;
                // pega o número de colunas preenchidas na primeira linha (cabeçalho)
                var totalColunas = planilha.Row(1).LastCellUsed().Address.ColumnNumber;
                if (totalColunas < 3)
                    throw new NegocioException(MensagemNegocioComuns.ARQUIVO_NUMERO_COLUNAS_INVALIDO);

                var totalLinhas = planilha.LastRowUsed().RowNumber();
                // utilizar o - 1 para desconsiderar o cabeçalho
                totalRegistros = totalLinhas - 1;
                var tamanhoLote = 10;

                if (totalLinhas <= 1)
                    throw new NegocioException(MensagemNegocioComuns.ARQUIVO_VAZIO);

                for (int linha = 2; linha <= totalLinhas; linha++)
                {
                    try
                    {
                        short.TryParse(planilha.Cell(linha, 1).Value.ToString().Trim(), out var serieAno);
                        var codigoUe = planilha.Cell(linha, 2).Value.ToString().Trim();
                        decimal.TryParse(planilha.Cell(linha, 3).Value.ToString().Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out var nota);

                        var dto = new ArquivoIdebDto(serieAno, codigoUe, nota, anoLetivo);
                        dto.LinhaAtual = linha;
                        listaLote.Add(dto);

                        if (listaLote.Count == tamanhoLote)
                        {
                            loteSalvar.AddRange(SalvarArquivoIdebEmLote(listaLote, importacaoLogDto.Id));
                            listaLote.Clear();
                        }
                    }
                    catch (Exception ex)
                    {
                        SalvarErroLinha(importacaoLogDto.Id, linha, ex.Message);
                    }
                }
                // salva as linhas restantes (último lote incompleto)
                if (listaLote.Any())
                    loteSalvar.AddRange(SalvarArquivoIdebEmLote(listaLote, importacaoLogDto.Id));

                // aguarda todas as tasks de persistência
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

        private IEnumerable<Task> SalvarArquivoIdebEmLote(List<ArquivoIdebDto> lista, long importacaoLogId)
        {
            var serieAnosValidos = new int[] {
                (int)SerieAnoIndiceDesenvolvimentoEnum.AnosIniciais,
                (int)SerieAnoIndiceDesenvolvimentoEnum.AnosFinais,
                (int)SerieAnoIndiceDesenvolvimentoEnum.EnsinoMedio
            };

            foreach (var dto in lista)
            {
                try
                {
                    if (dto.SerieAno == 0 || !serieAnosValidos.Contains(dto.SerieAno))
                    {
                        SalvarErroLinha(importacaoLogId, dto.LinhaAtual, "Série/Ano inválido");
                        continue;
                    }

                    if (dto.Nota == 0)
                    {
                        SalvarErroLinha(importacaoLogId, dto.LinhaAtual, "Nota inválida");
                        continue;
                    }

                    if (string.IsNullOrEmpty(dto.CodigoEOLEscola))
                    {
                        SalvarErroLinha(importacaoLogId, dto.LinhaAtual, "Código EOL da UE inválido");
                        continue;
                    }

                    if (!string.IsNullOrEmpty(dto.CodigoEOLEscola))
                    {
                        var ue = repositorioUeConsulta.ObterPorCodigo(dto.CodigoEOLEscola);
                        if (ue.EhNulo())
                        {
                            SalvarErroLinha(importacaoLogId, dto.LinhaAtual, "Código EOL da UE não encontrado");
                            continue;
                        }
                    }

                    mediator.Send(new SalvarImportacaoArquivoIdebCommand(dto)).GetAwaiter().GetResult();
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
