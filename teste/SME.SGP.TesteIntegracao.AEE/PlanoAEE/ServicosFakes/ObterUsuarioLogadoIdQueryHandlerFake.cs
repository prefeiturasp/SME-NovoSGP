using MediatR;
using SME.SGP.Aplicacao;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.PlanoAEE.ServicosFakes
{
    public class ObterUsuarioLogadoIdQueryHandlerFake : IRequestHandler<ObterUsuarioLogadoIdQuery, long>
    {
        private const long USUARIO_PAAI_LOGIN_4444444 = 4444444;
        public Task<long> Handle(ObterUsuarioLogadoIdQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(USUARIO_PAAI_LOGIN_4444444);
        }
    }
}
