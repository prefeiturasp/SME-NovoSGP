using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.ProficienciaIdep
{
    public class ObterProficienciaIdepPorAnoLetivoQueryHandler : IRequestHandler<ObterProficienciaIdepPorAnoLetivoQuery, IEnumerable<Dominio.ProficienciaIdep>>
    {
        private readonly IRepositorioProficienciaIdepConsulta repositorioProficienciaIdepConsulta;
        public ObterProficienciaIdepPorAnoLetivoQueryHandler(IRepositorioProficienciaIdepConsulta repositorio)
        {
            this.repositorioProficienciaIdepConsulta = repositorio ?? throw new System.ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<Dominio.ProficienciaIdep>> Handle(ObterProficienciaIdepPorAnoLetivoQuery request, CancellationToken cancellationToken)
             => await repositorioProficienciaIdepConsulta.ObterPorAnoLetivoCodigoUe(request.AnoLetivo, request.CodigoUe);
    }
}
