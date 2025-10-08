using MediatR;
using Prometheus;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterAbandono
{
    public class ObterAbandonoVisaoSmeDreQueryHandler : IRequestHandler<ObterAbandonoVisaoSmeDreQuery, IEnumerable<PainelEducacionalAbandono>>
    {
        private readonly IRepositorioPainelEducacionalAbandono repositorio;
        public ObterAbandonoVisaoSmeDreQueryHandler(IRepositorioPainelEducacionalAbandono repositorio)
        {
            this.repositorio = repositorio;
        }
        public async Task<IEnumerable<PainelEducacionalAbandono>> Handle(ObterAbandonoVisaoSmeDreQuery request, CancellationToken cancellationToken)
        {
            return await repositorio.ObterAbandonoVisaoSmeDre(request.AnoLetivo, request.CodigoDre, request.CodigoUe);
        }
    }
}
