using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao.Commands.ImportarArquivo;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.ImportarArquivo.Boletim;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.ImportarArquivo;
using SME.SGP.Infra.Enumerados;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.ImportarArquivo.Boletim
{
    public class BoletimIdepUseCase : AbstractUseCase, IBoletimIdepUseCase
    {
        private readonly IRepositorioImportacaoLog repositorioImportacaoLog;
        private List<SalvarImportacaoLogErroDto> ProcessadosComFalha;
        public BoletimIdepUseCase(IMediator mediator, IRepositorioImportacaoLog repositorioImportacaoLog) : base(mediator)
        {
            this.repositorioImportacaoLog = repositorioImportacaoLog;
        }

        public async Task<ImportacaoLogRetornoDto> Executar(int ano, IEnumerable<IFormFile> boletins)
        {
            if (ano == 0)
                throw new NegocioException("Informe o ano letivo.");

            if (boletins == null || boletins.Count() == 0)
                throw new NegocioException("Informe o arquivo .pdf");

       
            var importacaoLog = await SalvarImportacao();

            if (importacaoLog != null)
            {
                await ProcessarArquivoAsync(boletins, importacaoLog);
            }
            return ImportacaoLogRetornoDto.RetornarSucesso(MensagemNegocioComuns.ARQUIVO_IMPORTADO_COM_SUCESSO, importacaoLog.Id);
        }

        private async Task<bool> ProcessarArquivoAsync(IEnumerable<IFormFile> boletins, ImportacaoLog importacaoLog)
        {
            var listaLote = new List<ArquivoFluenciaLeitoraDto>();
            ProcessadosComFalha = new List<SalvarImportacaoLogErroDto>();
            int totalRegistros = 0;
            var loteSalvar = new List<Task>();

            try
            {
                foreach (var boletim in boletins)
                {
                    await mediator.Send(new ArmazenarArquivoFisicoCommand(boletim, boletim.Name, TipoArquivo.Importacao));
                }
            }
            catch (Exception ex)
            {
                SalvarErroLinha(importacaoLog.Id, 0, $"Erro geral na importação: {ex.Message}");
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

        private async Task<ImportacaoLog> SalvarImportacao()
        {
            var statusImportacao = SituacaoArquivoImportacao.CarregamentoInicial.GetAttribute<DisplayAttribute>().Name;
            var tipoArquivo = TipoArquivoImportacao.BOLETIM_IDEP.GetAttribute<DisplayAttribute>().Name;

            var importacaoLogDto = new SalvarImportacaoLogDto("Boletins IDEP", tipoArquivo, statusImportacao);

            return await mediator.Send(new SalvarImportacaoLogCommand(importacaoLogDto));
        }
    }
}
