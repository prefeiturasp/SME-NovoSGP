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
            this.mediator = mediator;
        }

        protected async Task<ImportacaoLog> SalvarImportacao(string nomeArquivo, string tipoArquivo)
        {
            var statusImportacao = SituacaoArquivoImportacao.CarregamentoInicial.GetAttribute<DisplayAttribute>().Name;

            var importacaoLogDto = new ImportacaoLogDto(nomeArquivo, tipoArquivo, statusImportacao);

            return await mediator.Send(new SalvarImportacaoLogCommand(importacaoLogDto));
        }

        protected async Task<ImportacaoLog> SalvarImportacao(IFormFile arquivo, string tipoArquivo)
        {
            var statusImportacao = SituacaoArquivoImportacao.CarregamentoInicial.GetAttribute<DisplayAttribute>().Name;

            var importacaoLogDto = new ImportacaoLogDto(arquivo.FileName, tipoArquivo, statusImportacao);

            return await mediator.Send(new SalvarImportacaoLogCommand(importacaoLogDto));
        }

        protected void SalvarErroLinha(long importacaoLogId, int linha, string mensagem)
        {
            var erro = new SalvarImportacaoLogErroDto(importacaoLogId, linha, mensagem);

            if (!processadosComFalha.Any(e => e.LinhaArquivo == erro.LinhaArquivo && e.MotivoFalha == erro.MotivoFalha))
            {
                processadosComFalha.Add(erro);
                mediator.Send(new SalvarImportacaoLogErroCommand(erro)).GetAwaiter().GetResult();
            }
        }

        protected async Task SalvarImportacaoLog(ImportacaoLogDto importacaoLogDto, int totalRegistros)
        {
            importacaoLogDto.TotalRegistros = totalRegistros;
            importacaoLogDto.RegistrosProcessados = totalRegistros - processadosComFalha.Count;
            importacaoLogDto.RegistrosComFalha = processadosComFalha.Count;
            importacaoLogDto.StatusImportacao = processadosComFalha.Count > 0
            ? SituacaoArquivoImportacao.ProcessadoComFalhas.GetAttribute<DisplayAttribute>().Name
            : SituacaoArquivoImportacao.ProcessadoComSucesso.GetAttribute<DisplayAttribute>().Name;
            importacaoLogDto.DataFimProcessamento = DateTime.Now;

            await mediator.Send(new SalvarImportacaoLogCommand(importacaoLogDto));
        }

        protected ImportacaoLogDto MapearParaDto(ImportacaoLog log)
        {
            var importacaoLogDto = new ImportacaoLogDto(log.NomeArquivo, log.TipoArquivoImportacao, log.StatusImportacao);
            importacaoLogDto.Id = log.Id;
            importacaoLogDto.DataInicioProcessamento = log.DataInicioProcessamento;
            importacaoLogDto.DataFimProcessamento = log.DataFimProcessamento;
            importacaoLogDto.TotalRegistros = log.TotalRegistros;
            importacaoLogDto.RegistrosProcessados = log.RegistrosProcessados;
            importacaoLogDto.RegistrosComFalha = log.RegistrosComFalha;
            importacaoLogDto.CriadoPor = log.CriadoPor;
            importacaoLogDto.CriadoRF = log.CriadoRF;
            importacaoLogDto.AlteradoPor = log.AlteradoPor;
            importacaoLogDto.AlteradoRF = log.AlteradoRF;

            return importacaoLogDto;
        }
    }
}
