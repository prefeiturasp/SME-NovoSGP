using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.UE.ObterModalidadesPorUeEAnoLetivo
{
    public class ObterModalidadesPorUeEAnoLetivoQueryHandler : IRequestHandler<ObterModalidadesPorUeEAnoLetivoQuery, IEnumerable<ModalidadeRetornoDto>>
    {
        private readonly IRepositorioUe repositorioUe;

        public ObterModalidadesPorUeEAnoLetivoQueryHandler(IRepositorioUe repositorioUe)
        {
            this.repositorioUe = repositorioUe;
        }

        public async Task<IEnumerable<ModalidadeRetornoDto>> Handle(ObterModalidadesPorUeEAnoLetivoQuery request, CancellationToken cancellationToken)
        {
            var listaModalidades = await repositorioUe.ObterModalidades(request.CodigoUe, request.AnoLetivo, request.ModadlidadesQueSeraoIgnoradas);
            if (!listaModalidades?.Any() ?? true) return new List<ModalidadeRetornoDto>();

            return from modalidade in listaModalidades
                   select new ModalidadeRetornoDto()
                   {
                       Id = (int)modalidade,
                       Nome = modalidade.GetAttribute<DisplayAttribute>().Name
                   };
        }
    }
}