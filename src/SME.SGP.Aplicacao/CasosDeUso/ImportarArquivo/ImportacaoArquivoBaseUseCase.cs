using MediatR;
using Microsoft.AspNetCore.Http;
using SME.SGP.Aplicacao.Commands.ImportarArquivo;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Enumerados;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.ImportarArquivo
{
    public abstract class ImportacaoArquivoBaseUseCase
    {
        protected readonly IMediator mediator;
        protected List<SalvarImportacaoLogErroDto> ProcessadosComFalha;

        protected ImportacaoArquivoBaseUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        protected async Task<ImportacaoLog> SalvarImportacao(string tipoArquivo)
        {
            var statusImportacao = SituacaoArquivoImportacao.CarregamentoInicial.GetAttribute<DisplayAttribute>().Name;

            var importacaoLogDto = new ImportacaoLogDto("Boletins IDEB", tipoArquivo, statusImportacao);

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

            if (!ProcessadosComFalha.Any(e => e.LinhaArquivo == erro.LinhaArquivo && e.MotivoFalha == erro.MotivoFalha))
            {
                ProcessadosComFalha.Add(erro);
                mediator.Send(new SalvarImportacaoLogErroCommand(erro)).GetAwaiter().GetResult();
            }
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

            return importacaoLogDto;
        }
    }
}
