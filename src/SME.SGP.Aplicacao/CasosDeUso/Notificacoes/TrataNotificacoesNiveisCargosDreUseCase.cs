using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class TrataNotificacoesNiveisCargosDreUseCase : AbstractUseCase, ITrataNotificacoesNiveisCargosDreUseCase
    {
        public TrataNotificacoesNiveisCargosDreUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            long.TryParse(param.Mensagem.ToString(), out long dreId);

            if (dreId > 0) 
            {
                var ues = await mediator.Send(new ObterUesPorDreCodigoQuery(dreId));

                foreach (var ue in ues)
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.TratarNotificacoesNiveisCargosUe, ue.CodigoUe, Guid.NewGuid(), null));
            }

            return true;
        }
    }
}
