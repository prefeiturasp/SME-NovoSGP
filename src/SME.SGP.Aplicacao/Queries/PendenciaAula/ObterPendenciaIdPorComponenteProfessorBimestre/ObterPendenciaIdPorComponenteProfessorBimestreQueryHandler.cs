using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciaIdPorComponenteProfessorBimestreQueryHandler : IRequestHandler<ObterPendenciaIdPorComponenteProfessorBimestreQuery, IEnumerable<PendenciaAulaProfessorDto>>
    {

        private readonly IRepositorioPendenciaAulaConsulta repositorioPendenciaAula;

        public ObterPendenciaIdPorComponenteProfessorBimestreQueryHandler(IRepositorioPendenciaAulaConsulta repositorioPendenciaAula)
        {
            this.repositorioPendenciaAula = repositorioPendenciaAula ?? throw new ArgumentNullException(nameof(repositorioPendenciaAula));
        }

        public async Task<IEnumerable<PendenciaAulaProfessorDto>> Handle(ObterPendenciaIdPorComponenteProfessorBimestreQuery request, CancellationToken cancellationToken)
            => await repositorioPendenciaAula.ObterPendenciaIdPorComponenteProfessorEBimestre(request.ComponenteCurricularId, request.CodigoRf, request.PeriodoEscolarId, request.TipoPendencia, request.TurmaCodigo, request.UeId);
    }
}
