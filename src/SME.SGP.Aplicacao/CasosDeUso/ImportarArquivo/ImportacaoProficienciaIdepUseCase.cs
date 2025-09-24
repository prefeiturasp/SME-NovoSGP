using ClosedXML.Excel;
using MediatR;
using Microsoft.AspNetCore.Http;
using SME.SGP.Aplicacao.Commands.ImportarArquivo;
using SME.SGP.Aplicacao.Commands.ImportarArquivo.ProficienciaIdep;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.ImportarArquivo;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Interfaces;
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
    public class ImportacaoProficienciaIdepUseCase : AbstractUseCase, IImportacaoProficienciaIdepUseCase
    {
        private readonly IRepositorioImportacaoLog repositorioImportacaoLog;
        private readonly IRepositorioUeConsulta repositorioUeConsulta;

        private List<SalvarImportacaoLogErroDto> ProcessadosComFalha;
        public ImportacaoProficienciaIdepUseCase(
            IMediator mediator,
            IRepositorioImportacaoLog repositorioImportacaoLog,
            IRepositorioUeConsulta repositorioUeConsulta
        ) : base(mediator)
        {
            this.repositorioImportacaoLog = repositorioImportacaoLog ?? throw new ArgumentNullException(nameof(repositorioImportacaoLog));
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

            var tipoArquivo = TipoArquivoImportacao.PROFICIENCIA_IDEP.GetAttribute<DisplayAttribute>().Name;
            var importacaoLog = await SalvarImportacao(arquivo, tipoArquivo);

            if (importacaoLog != null)
            {
                await ProcessarArquivoAsync(arquivo.OpenReadStream(), importacaoLog, anoLetivo);
            }
            return ImportacaoLogRetornoDto.RetornarSucesso(MensagemNegocioComuns.ARQUIVO_IMPORTADO_COM_SUCESSO, importacaoLog.Id);
        }

        private async Task<bool> ProcessarArquivoAsync(Stream arquivo, ImportacaoLog importacaoLog, int anoLetivo)
        {
            var listaLote = new List<ProficienciaIdepDto>();
            ProcessadosComFalha = new List<SalvarImportacaoLogErroDto>();
            int totalRegistros = 0;
            var loteSalvar = new List<Task>();

            try
            {
                using var package = new XLWorkbook(arquivo);
                var planilha = package.Worksheets.FirstOrDefault();
                if (planilha == null) return false;

                var totalColunas = planilha.Row(1).LastCellUsed().Address.ColumnNumber;
                if (totalColunas < 3)
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
                        int.TryParse(planilha.Cell(linha, 2).Value.ToString().Trim(), out int serieAno);
                        int.TryParse(planilha.Cell(linha, 3).Value.ToString().Trim(), out int componenteCurricular);
                        decimal.TryParse(planilha.Cell(linha, 4).Value.ToString().Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out var proficiencia);

                        var dto = new ProficienciaIdepDto(serieAno, codigoEOLEscola, anoLetivo, componenteCurricular, proficiencia);
                        dto.LinhaAtual = linha;
                        listaLote.Add(dto);

                        if (listaLote.Count == tamanhoLote)
                        {
                            loteSalvar.AddRange(SalvarArquivoProficienciaIdepEmLote(listaLote, importacaoLog.Id));
                            listaLote.Clear();
                        }
                    }
                    catch (Exception ex)
                    {
                        SalvarErroLinha(importacaoLog.Id, linha, ex.Message);
                    }
                }

                if (listaLote.Any())
                    loteSalvar.AddRange(SalvarArquivoProficienciaIdepEmLote(listaLote, importacaoLog.Id));

                await Task.WhenAll(loteSalvar);
            }
            catch (Exception ex)
            {
                SalvarErroLinha(importacaoLog.Id, 0, $"Erro geral no arquivo: {ex.Message}");
            }
            finally
            {
                importacaoLog.TotalRegistros = totalRegistros;
                importacaoLog.RegistrosProcessados = totalRegistros - ProcessadosComFalha.Count;
                importacaoLog.RegistrosComFalha = ProcessadosComFalha.Count;
                importacaoLog.StatusImportacao = ProcessadosComFalha.Count > 0
                ? SituacaoArquivoImportacao.ProcessadoComFalhas.GetAttribute<DisplayAttribute>().Name
                : SituacaoArquivoImportacao.ProcessadoComSucesso.GetAttribute<DisplayAttribute>().Name;
                importacaoLog.DataFimProcessamento = DateTime.Now;
                await repositorioImportacaoLog.SalvarAsync(importacaoLog);
            }
            return true;
        }

        private void SalvarErroLinha(long importacaoLogId, int linha, string mensagem)
        {
            var erro = new SalvarImportacaoLogErroDto(importacaoLogId, linha, mensagem);

            if (!ProcessadosComFalha.Any(e => e.LinhaArquivo == erro.LinhaArquivo && e.MotivoFalha == erro.MotivoFalha))
            {
                ProcessadosComFalha.Add(erro);
                mediator.Send(new SalvarImportacaoLogErroCommand(erro)).GetAwaiter().GetResult();
            }
        }

        private IEnumerable<Task> SalvarArquivoProficienciaIdepEmLote(List<ProficienciaIdepDto> lista, long importacaoLogId)
        {
            var serieAnosValidos = new int[] {
                (int)SerieAnoIdepEnum.AnosIniciais,
                (int)SerieAnoIdepEnum.AnosFinais
            };

            foreach (var dto in lista)
            {
                try
                {
                    if (dto.SerieAno <= 0 || !serieAnosValidos.Contains(dto.SerieAno))
                    {
                        SalvarErroLinha(importacaoLogId, dto.LinhaAtual, "Série/Ano inválido");
                        continue;
                    }

                    if (dto.Proficiencia <= 0)
                    {
                        SalvarErroLinha(importacaoLogId, dto.LinhaAtual, "Proficiencia inválida");
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

                    if (dto.ComponenteCurricular <= 0)
                    {
                        SalvarErroLinha(importacaoLogId, dto.LinhaAtual, "Componente curricular inválido");
                        continue;
                    }

                    mediator.Send(new ExcluirImportacaoProficienciaIdepPorAnoEscolaSerieCommand(dto.AnoLetivo, dto.CodigoEOLEscola, dto.SerieAno)).GetAwaiter().GetResult();
                    mediator.Send(new SalvarImportacaoProficienciaIdepCommand(dto)).GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    SalvarErroLinha(importacaoLogId, dto.LinhaAtual, ex.Message);
                }
            }

            return Enumerable.Empty<Task>();
        }

        private async Task<ImportacaoLog> SalvarImportacao(IFormFile arquivo, string tipoArquivo)
        {
            var statusImportacao = SituacaoArquivoImportacao.CarregamentoInicial.GetAttribute<DisplayAttribute>().Name;

            var importacaoLogDto = new ImportacaoLogDto(arquivo.FileName, tipoArquivo, statusImportacao);

            return await mediator.Send(new SalvarImportacaoLogCommand(importacaoLogDto));
        }
    }
}

