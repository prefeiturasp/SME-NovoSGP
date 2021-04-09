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
            var parametro = await repositorioParametrosSistema.ObterParametroPorTipoEAno(TipoParametroSistema.NovasModalidades, request.AnoLetivo);
            var novasModalidadesSplit = parametro?.Valor.Split(',');
            return novasModalidadesSplit?.Select(x => (Modalidade)Enum.Parse(typeof(Modalidade), x));
        }
    }
}