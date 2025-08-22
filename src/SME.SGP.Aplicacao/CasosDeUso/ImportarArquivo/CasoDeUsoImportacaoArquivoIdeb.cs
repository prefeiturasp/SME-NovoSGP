using ClosedXML.Excel;
using MediatR;
using Microsoft.AspNetCore.Http;
using SME.SGP.Aplicacao.Commands.ImportarArquivo;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.ImportarArquivo;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Interfaces.Repositorios;
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
    public class CasoDeUsoImportacaoArquivoIdeb : AbstractUseCase, ICasoDeUsoImportacaoArquivoIdeb
    {
        private readonly IRepositorioImportacaoLog repositorioImportacaoLog;

        private List<ImportacaoLogErroDto> ProcessadosComFalha;
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
            var listaImportacaoArquivoIdebDto = new List<ArquivoIdebDto>();
            ProcessadosComFalha = new List<ImportacaoLogErroDto>();
            var totalLinhas = 0;
            var tarefasSalvar = new List<Task>();

            try
            {
                // TODO: Validar se o arquivo é do tipo XLSX
                // TODO: Testar com arquivo sem  colunas
                // TODO: Testar com arquivo sem  linhas
                // TODO: Testar com arquivo sem  informação em algumas colunas
                using (var package = new XLWorkbook(arquivo))
                {
                    var planilha = package.Worksheets.FirstOrDefault();

                    if (planilha == null)
                        return false;

                    totalLinhas = planilha.LastRowUsed().RowNumber();
                    var quantidadeLinhasEmLote = 3; // virá das secrets/configMaps

                    for (int linha = 2; linha <= totalLinhas; linha++)
                    {
                        try
                        {
                            var codigoEOLEscola = planilha.ObterValorDaCelula(linha, COLUNA_CODIGO_EOL_ESCOLA_NUMERO);
                            var serieAno = int.Parse(planilha.ObterValorDaCelula(linha, COLUNA_SERIE_ANO_NUMERO));
                            var nota = Math.Round(decimal.Parse(planilha.ObterValorDaCelula(linha, COLUNA_NOTA_NUMERO), CultureInfo.InvariantCulture), 2);

                            var arquivoIdebDto = new ArquivoIdebDto(serieAno, codigoEOLEscola, nota);
                            arquivoIdebDto.LinhaAtual = linha;

                            listaImportacaoArquivoIdebDto.Add(arquivoIdebDto);

                            // atingiu o limite do lote
                            if (listaImportacaoArquivoIdebDto.Count == quantidadeLinhasEmLote)
                            {
                                tarefasSalvar.AddRange(SalvarArquivoIdebEmLote(listaImportacaoArquivoIdebDto, importacaoLog.Id));
                                listaImportacaoArquivoIdebDto.Clear();
                            }
                        }
                        catch (Exception ex)
                        {
                            ProcessadosComFalha.Add(new ImportacaoLogErroDto(importacaoLog.Id, linha, ex.Message));
                        }
                    }

                    // salva as linhas restantes (último lote incompleto)
                    if (listaImportacaoArquivoIdebDto.Any())
                        tarefasSalvar.AddRange(SalvarArquivoIdebEmLote(listaImportacaoArquivoIdebDto, importacaoLog.Id));

                    // aguarda todas as tasks de persistência
                    await Task.WhenAll(tarefasSalvar);
                }
            }
            catch (Exception ex)
            {
                ProcessadosComFalha.Add(new ImportacaoLogErroDto(importacaoLog.Id, 0, ex.Message));
            }
            finally
            {
                importacaoLog.TotalRegistros = totalLinhas - 1;
                importacaoLog.RegistrosProcessados = totalLinhas - ProcessadosComFalha.Count();
                importacaoLog.RegistrosComFalha = ProcessadosComFalha.Count();
                importacaoLog.DataFimProcessamento = DateTime.Now;
                await repositorioImportacaoLog.SalvarAsync(importacaoLog);
            }

            return true;
        }

        /// <summary>
        /// Cria uma lista de tasks, uma por registro do lote.
        /// </summary>
        private IEnumerable<Task> SalvarArquivoIdebEmLote(List<ArquivoIdebDto> listaImportacaoArquivoIdebDto, long importacaoLogId)
        {
            var tarefas = new List<Task>();
            foreach (var arquivoIdebDto in listaImportacaoArquivoIdebDto)
            {
                try
                {
                    var erros = ValidarRegistros(arquivoIdebDto, importacaoLogId, arquivoIdebDto.LinhaAtual);

                    if (erros.Any())
                        ProcessadosComFalha.AddRange(erros);
                    else
                        tarefas.Add(mediator.Send(new SalvarImportacaoArquivoIdebCommand(arquivoIdebDto, importacaoLogId)));

                }
                catch (Exception ex)
                {
                    ProcessadosComFalha.Add(new ImportacaoLogErroDto(importacaoLogId, arquivoIdebDto.LinhaAtual, ex.Message));
                }
            }
            return tarefas;
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

            // TODO: Verificar a dependencia com o EOL

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
