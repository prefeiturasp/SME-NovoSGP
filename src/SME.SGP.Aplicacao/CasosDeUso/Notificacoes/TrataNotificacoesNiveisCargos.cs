using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class TrataNotificacoesNiveisCargos : ITrataNotificacoesNiveisCargos
    {
        private readonly IMediator mediator;

        public TrataNotificacoesNiveisCargos(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }
        public async Task Executar()
        {
            var listaDeNotificacoesParaTratar = await mediator.Send(new ObterNotificacoesNiveisCargosQuery());

            throw new System.NotImplementedException();
        }
    }
}
