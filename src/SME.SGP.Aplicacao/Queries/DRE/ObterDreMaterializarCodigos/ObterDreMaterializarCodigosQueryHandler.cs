using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao 
{
    public class ObterDreMaterializarCodigosQueryHandler : IRequestHandler<ObterDreMaterializarCodigosQuery, Tuple<IEnumerable<Dre>, string[]>>
    {
        private readonly IRepositorioDreConsulta repositorioDre;

        public ObterDreMaterializarCodigosQueryHandler(IRepositorioDreConsulta repositorio)
        {
            this.repositorioDre = repositorio;
        }

        public async Task<Tuple<IEnumerable<Dre>, string[]>> Handle(ObterDreMaterializarCodigosQuery request, CancellationToken cancellationToken)
        {
            return repositorioDre.MaterializarCodigosDre(request.IdDres);
        }
            
    }
}