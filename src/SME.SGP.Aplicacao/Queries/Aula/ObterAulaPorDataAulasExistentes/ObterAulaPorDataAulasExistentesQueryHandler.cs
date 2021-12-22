using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAulaPorDataAulasExistentesQueryHandler : IRequestHandler<ObterAulaPorDataAulasExistentesQuery, IEnumerable<DateTime>>
    {
        private readonly IRepositorioAulaConsulta repositorioaulaConsulta;

        public ObterAulaPorDataAulasExistentesQueryHandler(IRepositorioAulaConsulta repositorio)
        {
            this.repositorioaulaConsulta = repositorio;
        }

        public async Task<IEnumerable<DateTime>> Handle(ObterAulaPorDataAulasExistentesQuery request, CancellationToken cancellationToken)
            => await repositorioaulaConsulta.ObterDatasAulasExistentes(request.DiasParaIncluirRecorrencia, request.TurmaCodigo, request.ComponenteCurricularCodigo, request.ProfessorCJ);
    }
}

