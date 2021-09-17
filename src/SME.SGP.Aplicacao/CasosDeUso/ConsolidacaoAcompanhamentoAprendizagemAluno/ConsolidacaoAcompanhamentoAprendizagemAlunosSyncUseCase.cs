using MediatR;
using Sentry;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsolidacaoAcompanhamentoAprendizagemAlunosSyncUseCase : AbstractUseCase, IConsolidacaoAcompanhamentoAprendizagemAlunosSyncUseCase
    {
        public ConsolidacaoAcompanhamentoAprendizagemAlunosSyncUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            try
            {
                if (!await ExecutarConsolidacao())
                    return false;

                await ConsolidarAcompanhamentoAprendizagemAluno();
                return true;
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw;
            }
        }

        private async Task ConsolidarAcompanhamentoAprendizagemAluno()
        {
            var anoAtual = DateTime.Now.Year;
            await mediator.Send(new LimparConsolidacaoAcompanhamentoAprendizagemCommand(anoAtual));

            var ues = await mediator.Send(new ObterCodigosUEsQuery());
            foreach (var ue in ues)
                await PublicarConsolidacaoPorUe(ue);

            await AtualizarDataExecucao(anoAtual);
        }

        private async Task PublicarConsolidacaoPorUe(string ue)
        {
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ConsolidarAcompanhamentoAprendizagemAlunoPorUE, new FiltroUEDto(ue), Guid.NewGuid(), null));
        }

        private async Task<bool> ExecutarConsolidacao()
        {
            var parametroExecucao = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.ExecucaoConsolidacaoAcompanhamentoAprendizagemAlunoTurma, DateTime.Now.Year));
            if (parametroExecucao != null)
                return parametroExecucao.Ativo;

            return false;
        }

        private async Task AtualizarDataExecucao(int ano)
        {
            var parametroSistema = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.ExecucaoConsolidacaoAcompanhamentoAprendizagemAlunoTurma, ano));
            if (parametroSistema != null)
            {
                parametroSistema.Valor = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff tt");
                await mediator.Send(new AtualizarParametroSistemaCommand(parametroSistema));
            }
        }
    }
}
