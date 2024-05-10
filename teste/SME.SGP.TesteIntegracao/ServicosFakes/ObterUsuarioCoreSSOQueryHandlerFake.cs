using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class ObterUsuarioCoreSSOQueryHandlerFake : IRequestHandler<ObterUsuarioCoreSSOQuery, MeusDadosDto>
    {
        public async Task<MeusDadosDto> Handle(ObterUsuarioCoreSSOQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new MeusDadosDto()
            {
                Nome = "João Usuário",
                Email = string.Empty
            });
        }
    }
}
