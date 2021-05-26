using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUesIdsPorDreQueryHeandler : IRequestHandler<ObterUesIdsPorDreQuery, IEnumerable<long>>
    {
        private readonly IRepositorioUe repositorioUe;

        public ObterUesIdsPorDreQueryHeandler(IRepositorioUe repositorioUe)
        {
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
        }

        public async Task<IEnumerable<long>> Handle(ObterUesIdsPorDreQuery request, CancellationToken cancellationToken)
        {
            return await repositorioUe.ObterUesIdsPorDreAsync(request.DreId);
        }
    }
}
