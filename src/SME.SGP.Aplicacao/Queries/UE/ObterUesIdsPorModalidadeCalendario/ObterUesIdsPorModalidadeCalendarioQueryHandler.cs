using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterUesIdsPorModalidadeCalendarioQueryHandler : IRequestHandler<ObterUesIdsPorModalidadeCalendarioQuery, IEnumerable<long>>
    {
        private readonly IRepositorioUeConsulta repositorioUe;

        public ObterUesIdsPorModalidadeCalendarioQueryHandler(IRepositorioUeConsulta repositorioUe)
        {
            this.repositorioUe = repositorioUe;
        }

        public Task<IEnumerable<long>> Handle(ObterUesIdsPorModalidadeCalendarioQuery request, CancellationToken cancellationToken)
        {
            var modalidades = request.ModalidadeTipoCalendario.ObterModalidadesTurma();

            return repositorioUe.ObterUesIdsPorModalidade(modalidades.Cast<int>().ToArray(), request.AnoLetivo);
        }
    }
}
