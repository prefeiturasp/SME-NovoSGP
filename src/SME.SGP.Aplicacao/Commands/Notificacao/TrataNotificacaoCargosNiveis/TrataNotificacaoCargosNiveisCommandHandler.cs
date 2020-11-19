using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class TrataNotificacaoCargosNiveisCommandHandler : IRequestHandler<TrataNotificacaoCargosNiveisCommand, bool>
    {
        private readonly IMediator mediator;

        public TrataNotificacaoCargosNiveisCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }
        public async Task<bool> Handle(TrataNotificacaoCargosNiveisCommand request, CancellationToken cancellationToken)
        {

            foreach (var notificacaoParaTratar in request.Notificacoes.GroupBy(a => a.UEId))
            {
                var cargosIdsDaUe = notificacaoParaTratar.Select(a => a.Cargo).Distinct();
                var funcionariosCargosDaUe = await mediator.Send(new ObterFuncionariosCargosPorUeCargosQuery(notificacaoParaTratar.Key, cargosIdsDaUe));
                
                //Verificar no EOL rfs e cargos na UE
            }

            return true;
        }
    }
}
