using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class TrataNotificacaoCargosNiveisCommandHandler : IRequestHandler<TrataNotificacaoCargosNiveisCommand, bool>
    {
        public async Task<bool> Handle(TrataNotificacaoCargosNiveisCommand request, CancellationToken cancellationToken)
        {

            foreach (var notificacaoParaTratar in request.Notificacoes.OrderBy(a => a.UEId))
            {

            }

            return true;
        }
    }
}
