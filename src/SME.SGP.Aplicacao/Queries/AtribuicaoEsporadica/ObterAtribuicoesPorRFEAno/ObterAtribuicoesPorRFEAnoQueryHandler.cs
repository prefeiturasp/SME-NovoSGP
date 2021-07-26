using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAtribuicoesPorRFEAnoQueryHandler : IRequestHandler<ObterAtribuicoesPorRFEAnoQuery, IEnumerable<AtribuicaoEsporadica>>
    {
        private readonly IRepositorioAtribuicaoEsporadica repositorioAtribuicaoEsporadica;

        public ObterAtribuicoesPorRFEAnoQueryHandler(IRepositorioAtribuicaoEsporadica repositorioAtribuicaoEsporadica)
        {
            this.repositorioAtribuicaoEsporadica = repositorioAtribuicaoEsporadica ?? throw new System.ArgumentNullException(nameof(repositorioAtribuicaoEsporadica));
        }

        public async Task<IEnumerable<AtribuicaoEsporadica>> Handle(ObterAtribuicoesPorRFEAnoQuery request, CancellationToken cancellationToken)
            => await repositorioAtribuicaoEsporadica.ObterAtribuicoesPorRFEAno(request.ProfessorRf, request.SomenteInfantil, request.AnoLetivo);

    }
}
