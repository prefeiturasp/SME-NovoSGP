using ClosedXML.Excel;
using MediatR;
using Microsoft.AspNetCore.Http;
using SME.SGP.Aplicacao.Commands.ImportarArquivo.FluenciaLeitora;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.ImportarArquivo.FluenciaLeitora;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.ImportarArquivo;
using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoFluenciaLeitoraUe;
using SME.SGP.Infra.Enumerados;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace SME.SGP.Aplicacao.CasosDeUso.ImportarArquivo.FluenciaLeitora
{
    public class ImportacaoArquivoFluenciaLeitoraUseCase : ImportacaoArquivoBaseUseCase, IImportacaoArquivoFluenciaLeitoraUseCase
    {
        private readonly IRepositorioTurmaConsulta repositorioTurmaConsulta;

        public ImportacaoArquivoFluenciaLeitoraUseCase(IMediator mediator, IRepositorioTurmaConsulta repositorioTurmaConsulta) : base(mediator)
        {
            this.repositorioTurmaConsulta = repositorioTurmaConsulta ?? throw new ArgumentNullException(nameof(repositorioTurmaConsulta));
        }

        public async Task<ImportacaoLogRetornoDto> Executar(IFormFile arquivo, int anoLetivo, int tipoAvaliacao)
        {
            if (anoLetivo == 0)
                throw new NegocioException("Informe o ano letivo.");

            var fluenciaLeitoraPeriodoValidos = new int[] {
                (int)FluenciaLeitoraTipoAvaliacaoEnum.AvaliacaoEntrada,
                (int)FluenciaLeitoraTipoAvaliacaoEnum.AvaliacaoSaida
            };

            if (!fluenciaLeitoraPeriodoValidos.Contains(tipoAvaliacao))
                throw new NegocioException("Informe o tipo de avaliação.");

            if (arquivo == null || arquivo.Length == 0)
                throw new NegocioException(MensagemNegocioComuns.ARQUIVO_VAZIO);

            if (arquivo.ContentType.NaoEhArquivoXlsx())
                throw new NegocioException(MensagemNegocioComuns.SOMENTE_ARQUIVO_XLSX_SUPORTADO);

            var tipoArquivo = TipoArquivoImportacao.FLUENCIA_LEITORA.GetAttribute<DisplayAttribute>().Name;
            var importacaoLog = await SalvarImportacao(arquivo, tipoArquivo);

            if (importacaoLog != null)
            {
                var importacaoLogDto = MapearParaDto(importacaoLog);
                var sucesso = await ProcessarArquivoAsync(arquivo.OpenReadStream(), importacaoLogDto, anoLetivo, tipoAvaliacao);

                if (sucesso)
                {
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpPainelEducacional.ConsolidarFluenciaLeitoraPainelEducacional, Guid.NewGuid(), null));                    
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpPainelEducacional.ConsolidarFluenciaLeitoraUePainelEducacional, new MensagemConsolidacaoFluenciaLeitoraUeDto(anoLetivo), Guid.NewGuid(), null));

                    return ImportacaoLogRetornoDto.RetornarSucesso(MensagemNegocioComuns.ARQUIVO_IMPORTADO_COM_SUCESSO);
                }

                return ImportacaoLogRetornoDto.RetornarFalha("Falha ao processar importação de fluência leitora.");
            }

            return ImportacaoLogRetornoDto.RetornarFalha("Falha ao iniciar importação de fluência leitora.");
        }

        private async Task<bool> ProcessarArquivoAsync(Stream arquivo, ImportacaoLogDto importacaoLogDto, int anoLetivo, int tipoAvaliacao)
        {
            var listaLote = new List<ArquivoFluenciaLeitoraDto>();
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
                            loteSalvar.AddRange(SalvarArquivoFluenciaLeitoraEmLote(listaLote, importacaoLogDto.Id));
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
                    loteSalvar.AddRange(SalvarArquivoFluenciaLeitoraEmLote(listaLote, importacaoLogDto.Id));

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

            return processadosComFalha.Count == 0;
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

                    mediator.Send(new SalvarImportacaoArquivoFluenciaLeitoraCommand(dto)).GetAwaiter().GetResult();
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
