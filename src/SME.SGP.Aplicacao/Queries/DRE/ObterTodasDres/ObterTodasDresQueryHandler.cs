using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTodasDresQueryHandler : IRequestHandler<ObterTodasDresQuery, IEnumerable<Dre>>
    {
        private readonly IRepositorioDreConsulta repositorioDre;

        public ObterTodasDresQueryHandler(IRepositorioDreConsulta repositorioDre)
        {
            this.repositorioDre = repositorioDre ?? throw new System.ArgumentNullException(nameof(repositorioDre));
        }

        public Task<IEnumerable<Dre>> Handle(ObterTodasDresQuery request, CancellationToken cancellationToken)
        {
            return repositorioDre.ObterTodas();
        }
    }
}
