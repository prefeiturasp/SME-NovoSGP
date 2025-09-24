using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIdebPorAnoSerie
{
    public class ObterIdebPorAnoSerieQueryHandler : IRequestHandler<ObterIdebPorAnoSerieQuery, IEnumerable<PainelEducacionalIdebDto>>
    {
        private readonly IRepositorioIdebPainelEducacionalConsulta repositorioIdebConsulta;
        public ObterIdebPorAnoSerieQueryHandler(IRepositorioIdebPainelEducacionalConsulta repositorioIdebConsulta)
        {
            this.repositorioIdebConsulta = repositorioIdebConsulta ?? throw new ArgumentNullException(nameof(repositorioIdebConsulta));
        }
        public async Task<IEnumerable<PainelEducacionalIdebDto>> Handle(ObterIdebPorAnoSerieQuery request, CancellationToken cancellationToken)
        {
            return await repositorioIdebConsulta.ObterIdebPorAnoSerie(request.AnoLetivo, request.Serie, request.CodigoDre, request.CodigoUe);
        }
    }
}
