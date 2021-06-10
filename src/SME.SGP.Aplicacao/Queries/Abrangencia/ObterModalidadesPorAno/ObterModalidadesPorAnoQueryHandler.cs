using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterModalidadesPorAnoQueryHandler : IRequestHandler<ObterModalidadesPorAnoQuery, IEnumerable<EnumeradoRetornoDto>>
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;

        public ObterModalidadesPorAnoQueryHandler(IRepositorioAbrangencia repositorioAbrangencia)
        {
            this.repositorioAbrangencia = repositorioAbrangencia;
        }

        public async Task<IEnumerable<EnumeradoRetornoDto>> Handle(ObterModalidadesPorAnoQuery request, CancellationToken cancellationToken)
        {
            var lista = await repositorioAbrangencia.ObterModalidades(request.Login, request.Perfil, request.AnoLetivo, request.ConsideraHistorico, request.ModadlidadesQueSeraoIgnoradas);

            var listaModalidades = from a in lista
                                   select new EnumeradoRetornoDto() { Id = a, Descricao = ((Modalidade)a).GetAttribute<DisplayAttribute>().Name };

            return listaModalidades.OrderBy(a => a.Descricao);
        }
    }
}