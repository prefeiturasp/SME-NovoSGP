using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciaIdPorComponenteProfessorBimestreQueryHandler : IRequestHandler<ObterPendenciaIdPorComponenteProfessorBimestreQuery, long>
    {

        private readonly IRepositorioPendenciaAulaConsulta repositorioPendenciaAula;

        public ObterPendenciaIdPorComponenteProfessorBimestreQueryHandler(IRepositorioPendenciaAulaConsulta repositorioPendenciaAula)
        {
            this.repositorioPendenciaAula = repositorioPendenciaAula ?? throw new ArgumentNullException(nameof(repositorioPendenciaAula));
        }

        public async Task<long> Handle(ObterPendenciaIdPorComponenteProfessorBimestreQuery request, CancellationToken cancellationToken)
        => await repositorioPendenciaAula.ObterPendenciaIdPorComponenteProfessorEBimestre(request.ComponenteCurricularId, request.CodigoRf, request.PeriodoEscolarId, request.TipoPendencia, request.TurmaCodigo, request.UeId);
    }
}
