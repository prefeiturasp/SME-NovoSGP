using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsolidarFrequenciaTurmasPorDREUseCase : AbstractUseCase, IConsolidarFrequenciaTurmasPorDREUseCase
    {
        public ConsolidarFrequenciaTurmasPorDREUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtro = mensagem.ObterObjetoMensagem<FiltroConsolidacaoFrequenciaTurmaPorDre>();

            var ues = await mediator.Send(new ObterUEsIdsPorDreQuery(filtro.DreId));
            foreach(var ue in ues)
            {
                var filtroUe = new FiltroConsolidacaoFrequenciaTurmaPorUe(filtro.Data, filtro.TipoConsolidado, ue, filtro.PercentualMinimo, filtro.PercentualMinimoInfantil);

                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFrequencia.ConsolidarFrequenciasTurmasPorUe, filtroUe, Guid.NewGuid(), null));
            }

            return true;
        }
    }
}
