using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDREPorIdQueryHandler : IRequestHandler<ObterDREPorIdQuery, Dre>
    {
        private readonly IRepositorioDreConsulta repositorioDre;

        public ObterDREPorIdQueryHandler(IRepositorioDreConsulta repositorioDre)
        {
            this.repositorioDre = repositorioDre ?? throw new ArgumentNullException(nameof(repositorioDre));
        }

        public async Task<long> Handle(ObterDREIdPorCodigoQuery request, CancellationToken cancellationToken)
        { 
            return await repositorioDre.ObterIdDrePorCodigo(request.CodigoDre);
        }

        public async Task<Dre> Handle(ObterDREPorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioDre.ObterPorIdAsync(request.DreId);
        }
    }
}
