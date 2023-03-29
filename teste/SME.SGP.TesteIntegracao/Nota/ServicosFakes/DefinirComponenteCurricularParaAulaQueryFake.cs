using MediatR;
using SME.SGP.Aplicacao;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Nota.ServicosFakes
{
    public class DefinirComponenteCurricularParaAulaQueryFake : IRequestHandler<DefinirComponenteCurricularParaAulaQuery, (long codigoComponente, long? codigoTerritorio)>
    {
        public async Task<(long codigoComponente, long? codigoTerritorio)> Handle(DefinirComponenteCurricularParaAulaQuery request, CancellationToken cancellationToken)
        {
            return (0, null);
        }
    }
}
