using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTipoEscolaPorDreEUeQueryHandler : IRequestHandler<ObterTipoEscolaPorDreEUeQuery, IEnumerable<TipoEscolaDto>>
    {
        private readonly IRepositorioTipoEscola repositorioTipoEscola;

        public ObterTipoEscolaPorDreEUeQueryHandler(IRepositorioTipoEscola repositorioTipoEscola)
        {
            this.repositorioTipoEscola = repositorioTipoEscola ?? throw new System.ArgumentNullException(nameof(repositorioTipoEscola));
        }

        public async Task<IEnumerable<TipoEscolaDto>> Handle(ObterTipoEscolaPorDreEUeQuery request, CancellationToken cancellationToken)
        {
            var tiposEscola = await repositorioTipoEscola.ObterTipoEscolaPorDreEUe(request.DreCodigo, request.UeCodigo);

            return tiposEscola.OrderBy(c => c.CodTipoEscola);
        }            
    }
}
