using MediatR;
using Microsoft.AspNetCore.Http;
using SME.SGP.Aplicacao.Commands.ImportarArquivo;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.ImportarArquivo;
using SME.SGP.Infra.Enumerados;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.ImportarArquivo
{
    public abstract class ImportacaoArquivoBaseUseCase
    {
        protected readonly IMediator mediator;
        protected List<SalvarImportacaoLogErroDto> processadosComFalha;

        protected ImportacaoArquivoBaseUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            processadosComFalha = new List<SalvarImportacaoLogErroDto>();
        }

        #region Métodos de Salvar Importação

        protected async Task<ImportacaoLog> SalvarImportacao(string nomeArquivo, string tipoArquivo)
        {
            var statusInicial = SituacaoArquivoImportacao.CarregamentoInicial.GetAttribute<DisplayAttribute>().Name;
            var importacaoLogDto = new ImportacaoLogDto(nomeArquivo, tipoArquivo, statusInicial);

            return await mediator.Send(new SalvarImportacaoLogCommand(importacaoLogDto));
        }

        protected async Task<ImportacaoLog> SalvarImportacao(IFormFile arquivo, string tipoArquivo)
        {
            var statusInicial = SituacaoArquivoImportacao.CarregamentoInicial.GetAttribute<DisplayAttribute>().Name;
            var importacaoLogDto = new ImportacaoLogDto(arquivo.FileName, tipoArquivo, statusInicial);

            return await mediator.Send(new SalvarImportacaoLogCommand(importacaoLogDto));
        }

        #endregion

        #region Métodos de Log de Erros

        protected void SalvarErroLinha(long importacaoLogId, int linha, string mensagem)
        {
            var erro = new SalvarImportacaoLogErroDto(importacaoLogId, linha, mensagem);

            if (!processadosComFalha.Any(e => e.LinhaArquivo == erro.LinhaArquivo && e.MotivoFalha == erro.MotivoFalha))
            {
                processadosComFalha.Add(erro);
                mediator.Send(new SalvarImportacaoLogErroCommand(erro)).GetAwaiter().GetResult();
            }
        }

        #endregion

        #region Finalização da Importação

        protected async Task SalvarImportacaoLog(ImportacaoLogDto importacaoLogDto, int totalRegistros)
        {
            importacaoLogDto.TotalRegistros = totalRegistros;
            importacaoLogDto.RegistrosProcessados = totalRegistros - processadosComFalha.Count;
            importacaoLogDto.RegistrosComFalha = processadosComFalha.Count;
            importacaoLogDto.DataFimProcessamento = DateTime.Now;

            importacaoLogDto.StatusImportacao = DefinirStatusImportacao(totalRegistros);

            await mediator.Send(new SalvarImportacaoLogCommand(importacaoLogDto));
        }

        private string DefinirStatusImportacao(int totalRegistros)
        {
            if (totalRegistros == 0 || processadosComFalha?.Count > 0)
                return SituacaoArquivoImportacao.ProcessadoComFalhas.GetAttribute<DisplayAttribute>().Name;

            return SituacaoArquivoImportacao.ProcessadoComSucesso.GetAttribute<DisplayAttribute>().Name;
        }

        #endregion

        #region Utilidades

        protected static ImportacaoLogDto MapearParaDto(ImportacaoLog log)
        {
            if (log == null) return null;

            return new ImportacaoLogDto(log.NomeArquivo, log.TipoArquivoImportacao, log.StatusImportacao)
            {
                Id = log.Id,
                DataInicioProcessamento = log.DataInicioProcessamento,
                DataFimProcessamento = log.DataFimProcessamento,
                TotalRegistros = log.TotalRegistros,
                RegistrosProcessados = log.RegistrosProcessados,
                RegistrosComFalha = log.RegistrosComFalha,
                CriadoPor = log.CriadoPor,
                CriadoRF = log.CriadoRF,
                AlteradoPor = log.AlteradoPor,
                AlteradoRF = log.AlteradoRF
            };
        }

        #endregion
    }
}
