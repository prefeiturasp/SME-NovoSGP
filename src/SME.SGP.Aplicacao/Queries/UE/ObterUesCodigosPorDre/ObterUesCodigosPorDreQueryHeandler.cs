using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUesCodigosPorDreQueryHeandler : IRequestHandler<ObterUesCodigosPorDreQuery, IEnumerable<string>>
    {
        private readonly IRepositorioUeConsulta repositorioUe;

        public ObterUesCodigosPorDreQueryHeandler(IRepositorioUeConsulta repositorioUe)
        {
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
        }

        public async Task<IEnumerable<string>> Handle(ObterUesCodigosPorDreQuery request, CancellationToken cancellationToken)
        {
            return await repositorioUe.ObterUesCodigosPorDreAsync(request.DreId);
        }
    }
}
