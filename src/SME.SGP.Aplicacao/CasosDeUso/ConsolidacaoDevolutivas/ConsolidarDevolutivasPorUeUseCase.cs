using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsolidarDevolutivasPorUeUseCase : AbstractUseCase, IConsolidarDevolutivasPorUeUseCase
    {
        public ConsolidarDevolutivasPorUeUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            if (!await ExecutarConsolidacaoDevolutivas())
                return false;

            await ObterUesConsolidarDevolutivas();

            return true;
        }

        private async Task ObterUesConsolidarDevolutivas()
        {
            var anoAtual = DateTime.Now.Year;
            var ues = await mediator.Send(new ObterUesIdsPorModalidadeCalendarioQuery(ModalidadeTipoCalendario.Infantil, anoAtual));
            await PublicarMensagemConsolidarDevolutivasPorTurmasInfantil(ues);

        }

        private async Task PublicarMensagemConsolidarDevolutivasPorTurmasInfantil(IEnumerable<long> ues)
        {
            foreach (var ue in ues)
            {
                try
                {
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ConsolidarDevolutivasPorTurmaInfantil, ue, Guid.NewGuid(), null));
                }
                catch (Exception ex)
                {
                    await mediator.Send(new SalvarLogViaRabbitCommand("Publicar Mensagem Consolidar Devolutivas Ue ", LogNivel.Critico, LogContexto.Devolutivas, ex.Message));
                }

            }
        }
        private async Task<bool> ExecutarConsolidacaoDevolutivas()
        {
            var parametroExecucao = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.ExecucaoConsolidacaoDevolutivasTurma, DateTime.Now.Year));
            if (parametroExecucao.NaoEhNulo())
                return parametroExecucao.Ativo;

            return false;
        }

    }
}
