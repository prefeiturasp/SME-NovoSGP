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
    public class ObterUEsComDREsPorModalidadeTipoCalendarioQueryHandler : IRequestHandler<ObterUEsComDREsPorModalidadeTipoCalendarioQuery, IEnumerable<Ue>>
    {
        private readonly IRepositorioUe repositorioUe;

        public ObterUEsComDREsPorModalidadeTipoCalendarioQueryHandler(IRepositorioUe repositorioUe)
        {
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
        }

        public async Task<IEnumerable<Ue>> Handle(ObterUEsComDREsPorModalidadeTipoCalendarioQuery request, CancellationToken cancellationToken)
        {
            var modalidades = request.ModalidadeTipoCalendario.ObterModalidadesTurma();

            return await repositorioUe.ObterUEsComDREsPorModalidadeTipoCalendarioQuery(modalidades.Cast<int>().ToArray(), request.AnoLetivo);
        }
    }
}
