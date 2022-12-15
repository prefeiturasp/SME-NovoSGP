using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.NotaFechamentoFinal.ServicosFakes
{
    public class ObterUsuarioCoreSSOQueryFake : IRequestHandler<ObterUsuarioCoreSSOQuery, MeusDadosDto>
    {
        public async Task<MeusDadosDto> Handle(ObterUsuarioCoreSSOQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new MeusDadosDto());
        }
    }
}
