using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.UE.ObterUEsPorModalidadeCalendario
{
    public class ObterUEsPorModalidadeCalendarioQueryHandler : IRequestHandler<ObterUEsPorModalidadeCalendarioQuery, IEnumerable<Ue>>
    {
        private readonly IRepositorioUe repositorioUe;

        public ObterUEsPorModalidadeCalendarioQueryHandler(IRepositorioUe repositorioUe)
        {
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
        }

        public async Task<IEnumerable<Ue>> Handle(ObterUEsPorModalidadeCalendarioQuery request, CancellationToken cancellationToken)
        {
            var modalidades = request.ModalidadeTipoCalendario.ObterModalidadesTurma();

            return await repositorioUe.ObterUesPorModalidade(modalidades.Cast<int>().ToArray());
        }
    }
}
