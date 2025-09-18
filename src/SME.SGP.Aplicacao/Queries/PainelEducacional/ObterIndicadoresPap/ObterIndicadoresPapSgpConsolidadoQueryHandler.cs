using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Threading;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIndicadoresPap
{
    public class ObterIndicadoresPapSgpConsolidadoQueryHandler : IRequestHandler<ObterIndicadoresPapSgpConsolidadoQuery, IEnumerable<ContagemDificuldadePorTipoDto>>
    {
        private readonly IRepositorioPapConsulta repositorioPapConsulta;

        public ObterIndicadoresPapSgpConsolidadoQueryHandler(IRepositorioPapConsulta repositorioPapConsulta)
        {
            this.repositorioPapConsulta = repositorioPapConsulta ?? throw new ArgumentNullException(nameof(repositorioPapConsulta));
        }

        public async Task<IEnumerable<ContagemDificuldadePorTipoDto>> Handle(ObterIndicadoresPapSgpConsolidadoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioPapConsulta.ObterContagemDificuldadesConsolidadaGeral();
        }
    }
}
