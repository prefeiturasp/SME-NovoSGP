using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.ParametroSistema.ObterNovasModalidadesPorAno
{
    public class ObterNovasModalidadesPorAnoQueryHandler : IRequestHandler<ObterNovasModalidadesPorAnoQuery, IEnumerable<Modalidade>>
    {
        private readonly IRepositorioParametrosSistema repositorioParametrosSistema;

        public ObterNovasModalidadesPorAnoQueryHandler(IRepositorioParametrosSistema repositorioParametrosSistema)
        {
            this.repositorioParametrosSistema = repositorioParametrosSistema;
        }

        public async Task<IEnumerable<Modalidade>> Handle(ObterNovasModalidadesPorAnoQuery request, CancellationToken cancellationToken)
        {
            if (request.ConsideraNovasModalidades && request.AnoLetivo == DateTime.Now.Year) 
                return null;

            var anoLetivo = ObterAnoParaConsulta(request);
            var novasModalidades = await repositorioParametrosSistema.ObterNovasModalidadesAPartirDoAno(anoLetivo);
            var novasModalidadesSplit = novasModalidades?.Split(',');
            return novasModalidadesSplit?.Select(x => (Modalidade)Enum.Parse(typeof(Modalidade), x));
        }

        /* Caso seja considerado as novas modalidades, apenas serão removidos as modalidades adicionadas em anos seguintes. */
        private int ObterAnoParaConsulta(ObterNovasModalidadesPorAnoQuery request) => request.ConsideraNovasModalidades ? request.AnoLetivo + 1 : request.AnoLetivo;
    }
}