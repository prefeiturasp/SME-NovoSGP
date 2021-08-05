using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PossuiAtribuicaoEsporadicaPorAnoDataQueryHandler : IRequestHandler<PossuiAtribuicaoEsporadicaPorAnoDataQuery, bool>
    {
        private readonly IRepositorioAtribuicaoEsporadica repositorioAtribuicaoEsporadica;

        public PossuiAtribuicaoEsporadicaPorAnoDataQueryHandler(IRepositorioAtribuicaoEsporadica repositorioAtribuicaoEsporadica)
        {
            this.repositorioAtribuicaoEsporadica = repositorioAtribuicaoEsporadica ?? throw new System.ArgumentNullException(nameof(repositorioAtribuicaoEsporadica));
        }

        public async Task<bool> Handle(PossuiAtribuicaoEsporadicaPorAnoDataQuery request, CancellationToken cancellationToken)
        {
            return await repositorioAtribuicaoEsporadica.PossuiAtribuicaoPorAnoData(request.AnoLetivo,
                                                                                    request.DreCodigo,
                                                                                    request.UeCodigo,
                                                                                    request.ProfessorRf,
                                                                                    request.Data);
        }
    }
}
