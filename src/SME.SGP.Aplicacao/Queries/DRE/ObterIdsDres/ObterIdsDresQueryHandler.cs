using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterIdsDresQueryHandler : IRequestHandler<ObterIdsDresQuery, IEnumerable<long>>
    {
        private readonly IRepositorioDre repositorioDre;

        public ObterIdsDresQueryHandler(IRepositorioDre repositorioDre)
        {
            this.repositorioDre = repositorioDre ?? throw new System.ArgumentNullException(nameof(repositorioDre));
        }

        public async Task<IEnumerable<long>> Handle(ObterIdsDresQuery request, CancellationToken cancellationToken)
        {
            return await repositorioDre.ObterIdsDresAsync();
        }
    }
}
