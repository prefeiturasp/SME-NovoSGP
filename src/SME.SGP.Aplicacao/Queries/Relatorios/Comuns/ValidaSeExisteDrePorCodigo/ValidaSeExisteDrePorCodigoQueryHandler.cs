using System;
using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao.Queries
{
    public class ValidaSeExisteDrePorCodigoQueryHandler : IRequestHandler<ValidaSeExisteDrePorCodigoQuery, bool>
    {
        private readonly IRepositorioDre repositorioDre;

        public ValidaSeExisteDrePorCodigoQueryHandler(IRepositorioDre repositorioDre)
        {
            this.repositorioDre = repositorioDre ?? throw new ArgumentNullException(nameof(repositorioDre));
        }

        public async Task<bool> Handle(ValidaSeExisteDrePorCodigoQuery request, CancellationToken cancellationToken)
        {
            if (repositorioDre.ObterPorCodigo(request.CodigoDre) == null)
                throw new NegocioException("Não foi possível encontrar a DRE");

            return true;
        }
    }
}
