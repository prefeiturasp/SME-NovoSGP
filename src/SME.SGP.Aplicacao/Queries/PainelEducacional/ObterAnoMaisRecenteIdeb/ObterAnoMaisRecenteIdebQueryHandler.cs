using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterAnoMaisRecenteIdeb
{
    public class ObterAnoMaisRecenteIdebQueryHandler : IRequestHandler<ObterAnoMaisRecenteIdebQuery, int?>
    {
        private readonly IRepositorioIdebPainelEducacionalConsulta repositorioIdebConsulta;

        public ObterAnoMaisRecenteIdebQueryHandler(IRepositorioIdebPainelEducacionalConsulta repositorioIdebConsulta)
        {
            this.repositorioIdebConsulta = repositorioIdebConsulta ?? throw new ArgumentNullException(nameof(repositorioIdebConsulta));
        }

        public async Task<int?> Handle(ObterAnoMaisRecenteIdebQuery request, CancellationToken cancellationToken)
        {
            return await repositorioIdebConsulta.ObterAnoMaisRecenteIdeb(request.Serie, request.CodigoDre, request.CodigoUe);
        }
    }
}