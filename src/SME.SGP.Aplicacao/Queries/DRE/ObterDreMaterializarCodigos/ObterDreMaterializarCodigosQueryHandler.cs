using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao 
{
    public class ObterDreMaterializarCodigosQueryHandler : IRequestHandler<ObterDreMaterializarCodigosQuery, (IEnumerable<Dre> Dres, string[] CodigosDresNaoEncontrados)>
    {
        private readonly IRepositorioDreConsulta repositorioDre;

        public ObterDreMaterializarCodigosQueryHandler(IRepositorioDreConsulta repositorio)
        {
            this.repositorioDre = repositorio;
        }

        public async Task<(IEnumerable<Dre> Dres, string[] CodigosDresNaoEncontrados)> Handle(ObterDreMaterializarCodigosQuery request, CancellationToken cancellationToken)
        {
            return repositorioDre.MaterializarCodigosDre(request.IdDres);
        }
            
    }
}