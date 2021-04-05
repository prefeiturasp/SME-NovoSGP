using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDREIdPorCodigoQueryHandler : IRequestHandler<ObterDREIdPorCodigoQuery, long>
    {
        private readonly IRepositorioDre repositorioDre;

        public ObterDREIdPorCodigoQueryHandler(IRepositorioDre repositorioDre)
        {
            this.repositorioDre = repositorioDre ?? throw new ArgumentNullException(nameof(repositorioDre));
        }

        public async Task<long> Handle(ObterDREIdPorCodigoQuery request, CancellationToken cancellationToken)
        { 
            return await repositorioDre.ObterIdDrePorCodigo(request.CodigoDre);
        }
    }
}
