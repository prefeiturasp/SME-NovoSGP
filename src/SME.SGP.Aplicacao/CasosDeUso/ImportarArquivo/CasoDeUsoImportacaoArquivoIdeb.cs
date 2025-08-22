using ClosedXML.Excel;
using MediatR;
using Microsoft.AspNetCore.Http;
using SME.SGP.Aplicacao.Commands.ImportarArquivo;
using SME.SGP.Aplicacao.Commands.ImportarArquivo.Ideb;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.ImportarArquivo;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.ImportarArquivo;
using SME.SGP.Infra.Enumerados;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace SME.SGP.Aplicacao.CasosDeUso.ImportarArquivo
{
    public class CasoDeUsoImportacaoArquivoIdeb : AbstractUseCase, ICasoDeUsoImportacaoArquivoIdeb
    {
        private const int INICIO_LINHA_TITULO = 1;
        private const int INICIO_LINHA_DADOS = 2;

        private const string COLUNA_SERIE_ANO = "Série";
        private const string COLUNA_CODIGO_EOL_ESCOLA = "Código Escola";
        private const string COLUNA_NOTA = "Nota";

        private const int COLUNA_SERIE_ANO_NUMERO = 1;
        private const int COLUNA_CODIGO_EOL_ESCOLA_NUMERO = 2;
        private const int COLUNA_NOTA_NUMERO = 3;

        private readonly IRepositorioImportacaoLog repositorioImportacaoLog;
        public CasoDeUsoImportacaoArquivoIdeb(
            IMediator mediator,
            IRepositorioImportacaoLog repositorioImportacaoLog
        ) : base(mediator)
        {
            this.repositorioImportacaoLog = repositorioImportacaoLog ?? throw new ArgumentNullException(nameof(repositorioImportacaoLog));
        }

        public async Task<RetornoDto> Executar(IFormFile arquivo)
        {
            var tipoArquivo = TipoArquivoImportacao.IDEB.GetAttribute<DisplayAttribute>().Name;
            var importacaoLog = await SalvarImportacao(arquivo, tipoArquivo);

            if (importacaoLog != null)
            {
                await ProcessarArquivoAsync(arquivo.OpenReadStream(), importacaoLog);
            }
            return RetornoDto.RetornarSucesso(MensagemNegocioComuns.ARQUIVO_IMPORTADO_COM_SUCESSO, importacaoLog.Id);
        }

        public async Task<bool> ProcessarArquivoAsync(Stream arquivo, ImportacaoLog importacaoLog)
        {
            var processadosComSucesso = new List<ArquivoIdebDto>();
            var processadosComFalha = new List<ImportacaoLogErroDto>();

            using (var package = new XLWorkbook(arquivo))
            {
                var planilha = package.Worksheets.FirstOrDefault();

                if (planilha == null)
                    return false;

                var totalLinhas = planilha.LastRowUsed().RowNumber();

                var linhasParaProcessar = Enumerable.Range(INICIO_LINHA_DADOS, totalLinhas - INICIO_LINHA_DADOS + 1).ToList();
                var quantidadeLinhasEmLote = 10; // virá das secrets/configMaps

                foreach (var loteDeLinhas in linhasParaProcessar.Chunk(quantidadeLinhasEmLote))
                {
                    var tasks = loteDeLinhas.Select(numeroLinha => Task.Run(() =>
                    {
                        try
                        {
                            var codigoEOLEscola = planilha.ObterValorDaCelula(numeroLinha, COLUNA_CODIGO_EOL_ESCOLA_NUMERO);
                            var serieAno = int.Parse(planilha.ObterValorDaCelula(numeroLinha, COLUNA_SERIE_ANO_NUMERO));
                            var nota = Math.Round(decimal.Parse(planilha.ObterValorDaCelula(numeroLinha, COLUNA_NOTA_NUMERO), CultureInfo.InvariantCulture), 2);
                            var arquivoIdebDto = new ArquivoIdebDto(serieAno, codigoEOLEscola, nota);

                            var erros = ValidarRegistros(arquivoIdebDto, importacaoLog.Id, numeroLinha);

                            if (erros.Any())
                                processadosComFalha.AddRange(erros);

                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        //finally
                        //{
                        //    importacaoLog.TotalRegistros = totalLinhas - 1;
                        //    importacaoLog.RegistrosProcessados = processadosComSucesso.Count();
                        //    importacaoLog.RegistrosComFalha = processadosComFalha.Count();
                        //    importacaoLog.DataFimProcessamento = DateTime.Now;
                        //    await repositorioImportacaoLog.SalvarAsync(importacaoLog);
                        //}
                    })).ToList();

                    await Task.WhenAll(tasks);
                }
            }
            return true;
        }

        private List<ImportacaoLogErroDto> ValidarRegistros(ArquivoIdebDto arquivoIdeb, long importacaoLogId, long numeroLinha)
        {
            var erros = new List<ImportacaoLogErroDto>();
            var serieAnosValidos = new int[] {
                (int)SerieAnoArquivoIdebIdepEnum.AnosIniciais,
                (int)SerieAnoArquivoIdebIdepEnum.AnosFinais,
                (int)SerieAnoArquivoIdebIdepEnum.EnsinoMedio
            };

            if (!serieAnosValidos.Contains(arquivoIdeb.SerieAno))
                erros.Add(new ImportacaoLogErroDto(importacaoLogId, numeroLinha, "Série/Ano inválido."));

            if (arquivoIdeb.Nota > 0)
                erros.Add(new ImportacaoLogErroDto(importacaoLogId, numeroLinha, "Nota inválida."));

            if (string.IsNullOrWhiteSpace(arquivoIdeb.CodigoEOLEscola))
                erros.Add(new ImportacaoLogErroDto(importacaoLogId, numeroLinha, "Código EOL da Escola não pode ser nulo."));

            return erros;
        }

        private async Task<ImportacaoLog> SalvarImportacao(IFormFile arquivo, string tipoArquivo)
        {
            var statusImportacao = SituacaoArquivoImportacao.CarregamentoInicial.GetAttribute<DisplayAttribute>().Name;

            var importacaoLogDto = new ImportacaoLogDto(arquivo, tipoArquivo, statusImportacao);

            return await mediator.Send(new SalvarImportacaoLogCommand(importacaoLogDto));
        }
    }
}
