using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIndicadoresPap
{
    public class ObterIndicadoresPapSgpConsolidadoQueryHandler : IRequestHandler<ObterIndicadoresPapSgpConsolidadoQuery, IEnumerable<ContagemDificuldadeIndicadoresPapPorTipoDto>>
    {
        private readonly IRepositorioPapConsulta repositorioPapConsulta;

        public ObterIndicadoresPapSgpConsolidadoQueryHandler(IRepositorioPapConsulta repositorioPapConsulta)
        {
            this.repositorioPapConsulta = repositorioPapConsulta ?? throw new ArgumentNullException(nameof(repositorioPapConsulta));
        }

        public async Task<IEnumerable<ContagemDificuldadeIndicadoresPapPorTipoDto>> Handle(ObterIndicadoresPapSgpConsolidadoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioPapConsulta.ObterContagemDificuldadesConsolidadaGeral(request.AnoLetivo);
        }
    }
}
