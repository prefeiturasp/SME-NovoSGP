using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.ProficienciaIdeb
{
    public class ObterProficienciaIdebPorAnoLetivoQueryHandler : IRequestHandler<ObterProficienciaIdebPorAnoLetivoQuery, IEnumerable<Dominio.Entidades.ProficienciaIdeb>>
    {
        private readonly IRepositorioProficienciaIdebConsulta repositorioProficienciaIdebConsulta;
        public ObterProficienciaIdebPorAnoLetivoQueryHandler(IRepositorioProficienciaIdebConsulta repositorio)
        {
            this.repositorioProficienciaIdebConsulta = repositorio ?? throw new System.ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<Dominio.Entidades.ProficienciaIdeb>> Handle(ObterProficienciaIdebPorAnoLetivoQuery request, CancellationToken cancellationToken)
          => await repositorioProficienciaIdebConsulta.ObterPorAnoLetivoCodigoUe(request.AnoLetivo, request.CodigoUe);
    }
}
