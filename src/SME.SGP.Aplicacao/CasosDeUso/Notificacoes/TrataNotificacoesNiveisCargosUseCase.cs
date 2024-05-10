using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class TrataNotificacoesNiveisCargosUseCase : AbstractUseCase, ITrataNotificacoesNiveisCargosUseCase
    {
        public TrataNotificacoesNiveisCargosUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var dres = await mediator.Send(ObterTodasDresQuery.Instance);

            foreach(var dre in dres) 
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.TratarNotificacoesNiveisCargosDre, dre.Id, Guid.NewGuid(), null));
            
            return true;
        }

    }
}

