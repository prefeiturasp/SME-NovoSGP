using MediatR;
using SME.SGP.Aplicacao;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Autenticar
{
    public class ObterLoginAtualAutenticacaoQueryHandlerFake : IRequestHandler<ObterLoginAtualQuery, string>
    {
        public async Task<string> Handle(ObterLoginAtualQuery request, CancellationToken cancellationToken)
         => await Task.FromResult("PROFINF1");
    }
}
