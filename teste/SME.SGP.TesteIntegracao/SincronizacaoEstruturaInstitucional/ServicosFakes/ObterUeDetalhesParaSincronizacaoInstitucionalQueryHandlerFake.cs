using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.SincronizacaoEstruturaInstitucional.ServicosFakes
{
    public class ObterUeDetalhesParaSincronizacaoInstitucionalQueryHandlerFake : IRequestHandler<ObterUeDetalhesParaSincronizacaoInstitucionalQuery, UeDetalhesParaSincronizacaoInstituicionalDto>
    {
        public async Task<UeDetalhesParaSincronizacaoInstituicionalDto> Handle(ObterUeDetalhesParaSincronizacaoInstitucionalQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new UeDetalhesParaSincronizacaoInstituicionalDto()
            {
                DreCodigo = 2,
                UeNome = "UE 1"
            });
        }
    }
}
