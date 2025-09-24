using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Spreadsheet;
using Elasticsearch.Net;
using MediatR;
using Microsoft.AspNetCore.Http;
using SME.SGP.Aplicacao.Commands.ImportarArquivo;
using SME.SGP.Aplicacao.Commands.ImportarArquivo.FluenciaLeitora;
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
    public class ImportacaoArquivoFluenciaLeitoraUseCase : AbstractUseCase, IImportacaoArquivoFluenciaLeitoraUseCase
    {
        private readonly IRepositorioImportacaoLog repositorioImportacaoLog;
        private readonly IRepositorioTurmaConsulta repositorioTurmaConsulta;

        private List<SalvarImportacaoLogErroDto> ProcessadosComFalha;
        public ImportacaoArquivoFluenciaLeitoraUseCase(
            IMediator mediator,
            IRepositorioImportacaoLog repositorioImportacaoLog,
            IRepositorioTurmaConsulta repositorioTurmaConsulta
        ) : base(mediator)
        {
            this.repositorioImportacaoLog = repositorioImportacaoLog ?? throw new ArgumentNullException(nameof(repositorioImportacaoLog));
            this.repositorioTurmaConsulta = repositorioTurmaConsulta ?? throw new ArgumentNullException(nameof(repositorioTurmaConsulta));
        }

        public async Task<ImportacaoLogRetornoDto> Executar(IFormFile arquivo, int anoLetivo, int tipoAvaliacao)
        {
            if (anoLetivo == 0)
                throw new NegocioException("Informe o ano letivo.");

            if (arquivo == null || arquivo.Length == 0)
                throw new NegocioException(MensagemNegocioComuns.ARQUIVO_VAZIO);

            if (arquivo.ContentType.NaoEhArquivoXlsx())
                throw new NegocioException(MensagemNegocioComuns.SOMENTE_ARQUIVO_XLSX_SUPORTADO);

            var tipoArquivo = TipoArquivoImportacao.FLUENCIA_LEITORA.GetAttribute<DisplayAttribute>().Name;
            var importacaoLog = await SalvarImportacao(arquivo, tipoArquivo);

            if (importacaoLog != null)
            {
                await ProcessarArquivoAsync(arquivo.OpenReadStream(), importacaoLog, anoLetivo, tipoAvaliacao);
            }
            return ImportacaoLogRetornoDto.RetornarSucesso(MensagemNegocioComuns.ARQUIVO_IMPORTADO_COM_SUCESSO, importacaoLog.Id);
        }

        private async Task<bool> ProcessarArquivoAsync(Stream arquivo, ImportacaoLog importacaoLog, int anoLetivo, int tipoAvaliacao)
        {
            var listaLote = new List<ArquivoFluenciaLeitoraDto>();
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
                        var codigoEOLTurma = planilha.Cell(linha, 1).Value.ToString().Trim();
                        var codigoEOLAluno = planilha.Cell(linha, 2).Value.ToString().Trim();
                        int.TryParse(planilha.Cell(linha, 3).Value.ToString().Trim(), out int fluencia);

                        var dto = new ArquivoFluenciaLeitoraDto(codigoEOLTurma, codigoEOLAluno, anoLetivo, fluencia, tipoAvaliacao);
                        dto.LinhaAtual = linha;
                        listaLote.Add(dto);

                        if (listaLote.Count == tamanhoLote)
                        {
                            loteSalvar.AddRange(SalvarArquivoFluenciaLeitoraEmLote(listaLote, importacaoLog.Id));
                            listaLote.Clear();
                        }
                    }
                    catch (Exception ex)
                    {
                        SalvarErroLinha(importacaoLog.Id, linha, ex.Message);
                    }
                }
                // salva as linhas restantes (último lote incompleto)
                if (listaLote.Any())
                    loteSalvar.AddRange(SalvarArquivoFluenciaLeitoraEmLote(listaLote, importacaoLog.Id));

                // aguarda todas as tasks de persistência
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

        private IEnumerable<Task> SalvarArquivoFluenciaLeitoraEmLote(List<ArquivoFluenciaLeitoraDto> lista, long importacaoLogId)
        {
            var fluenciasLeitorasValidas = new int[] {
                (int)FluenciaLeitoraEnum.Fluencia1,
                (int)FluenciaLeitoraEnum.Fluencia2,
                (int)FluenciaLeitoraEnum.Fluencia3,
                (int)FluenciaLeitoraEnum.Fluencia4,
                (int)FluenciaLeitoraEnum.Fluencia5,
                (int)FluenciaLeitoraEnum.Fluencia6,
            };

            var fluenciaLeitoraPeriodoValidos = new int[] {
                (int)FluenciaLeitoraTipoAvaliacaoEnum.AvaliacaoEntrada,
                (int)FluenciaLeitoraTipoAvaliacaoEnum.AvaliacaoSaida
            };

            foreach (var dto in lista)
            {
                try
                {
                    if (string.IsNullOrEmpty(dto.CodigoEOLTurma))
                    {
                        SalvarErroLinha(importacaoLogId, dto.LinhaAtual, "Codigo EOL da Turma inválido");
                        continue;
                    }

                    if (string.IsNullOrEmpty(dto.CodigoEOLAluno))
                    {
                        SalvarErroLinha(importacaoLogId, dto.LinhaAtual, "Codigo EOL do Aluno inválido");
                        continue;
                    }

                    if (!string.IsNullOrEmpty(dto.CodigoEOLTurma))
                    {
                        var turma = repositorioTurmaConsulta.ObterPorCodigo(dto.CodigoEOLTurma);
                        if (turma.EhNulo())
                        {
                            SalvarErroLinha(importacaoLogId, dto.LinhaAtual, "Turma não encontrada");
                            continue;
                        }
                    }

                    if (!string.IsNullOrEmpty(dto.CodigoEOLTurma) && !string.IsNullOrEmpty(dto.CodigoEOLAluno))
                    {
                        var aluno = mediator.Send(new ObterMatriculasAlunoNaTurmaQuery(dto.CodigoEOLTurma, dto.CodigoEOLAluno));
                        if (aluno.EhNulo())
                        {
                            SalvarErroLinha(importacaoLogId, dto.LinhaAtual, "Aluno não encontrado");
                            continue;
                        }
                    }

                    if (dto.Fluencia == 0 || !fluenciasLeitorasValidas.Contains(dto.Fluencia))
                    {
                        SalvarErroLinha(importacaoLogId, dto.LinhaAtual, "Fluência inválida");
                        continue;
                    }

                    if (dto.TipoAvaliacao == 0 || !fluenciaLeitoraPeriodoValidos.Contains(dto.TipoAvaliacao))
                    {
                        SalvarErroLinha(importacaoLogId, dto.LinhaAtual, "Período inválido");
                        continue;
                    }

                    mediator.Send(new SalvarImportacaoArquivoFluenciaLeitoraCommand(dto)).GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    SalvarErroLinha(importacaoLogId, dto.LinhaAtual, ex.Message);
                }
            }

            return Enumerable.Empty<Task>(); // mantém assinatura
        }

        private async Task<ImportacaoLog> SalvarImportacao(IFormFile arquivo, string tipoArquivo)
        {
            var statusImportacao = SituacaoArquivoImportacao.CarregamentoInicial.GetAttribute<DisplayAttribute>().Name;

            var importacaoLogDto = new SalvarImportacaoLogDto(arquivo.FileName, tipoArquivo, statusImportacao);

            return await mediator.Send(new SalvarImportacaoLogCommand(importacaoLogDto));
        }
    }
}
