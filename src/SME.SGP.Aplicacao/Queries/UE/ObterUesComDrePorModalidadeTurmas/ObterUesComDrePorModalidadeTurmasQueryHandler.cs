using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUesComDrePorModalidadeTurmasQueryHandler : IRequestHandler<ObterUesComDrePorModalidadeTurmasQuery, IEnumerable<Ue>>
    {
        private readonly IRepositorioUe repositorioUe;

        public ObterUesComDrePorModalidadeTurmasQueryHandler(IRepositorioUe repositorioUe)
        {
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
        }

        public async Task<IEnumerable<Ue>> Handle(ObterUesComDrePorModalidadeTurmasQuery request, CancellationToken cancellationToken)
            => await repositorioUe.ObterUesPorModalidade(new int[] { (int)request.Modalidade }, request.Ano);
    }
}
