using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class TrataNotificacoesNiveisCargosUeUseCase : AbstractUseCase, ITrataNotificacoesNiveisCargosUeUseCase
    {
        public TrataNotificacoesNiveisCargosUeUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var codigoUe = param.Mensagem.ToString();

            if (!string.IsNullOrEmpty(codigoUe))
            {
                var listaDeNotificacoesParaTratar = await mediator.Send(new ObterNotificacoesNiveisCargosQuery(codigoUe));
                await mediator.Send(new TrataNotificacaoCargosNiveisCommand(listaDeNotificacoesParaTratar));
            }

            return true;
        }
    }
}
