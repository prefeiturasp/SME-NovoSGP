using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsolidarReflexoFrequenciaBuscaAtivaDreUseCase : AbstractUseCase, IConsolidarReflexoFrequenciaBuscaAtivaDreUseCase
    {
        public ConsolidarReflexoFrequenciaBuscaAtivaDreUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtro = mensagem.ObterObjetoMensagem<FiltroIdAnoLetivoDto>();
            var ues = await mediator.Send(new ObterUEsIdsPorDreQuery(filtro.Id));
            foreach(long UeId in ues)
            {
                filtro.Id = UeId;
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFrequencia.ConsolidarReflexoFrequenciaBuscaAtivaUe, filtro, Guid.NewGuid()));
            }
            return true;
        }
    }
}
