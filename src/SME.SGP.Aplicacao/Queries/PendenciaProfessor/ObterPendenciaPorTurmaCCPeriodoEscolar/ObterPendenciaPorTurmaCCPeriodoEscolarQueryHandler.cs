using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciaPorTurmaCCPeriodoEscolarQueryHandler : IRequestHandler<ObterPendenciaPorTurmaCCPeriodoEscolarQuery, long>
    {
        private readonly IRepositorioPendenciaProfessor repositorioPendenciaProfessor;

        public ObterPendenciaPorTurmaCCPeriodoEscolarQueryHandler(IRepositorioPendenciaProfessor repositorioPendenciaProfessor)
        {
            this.repositorioPendenciaProfessor = repositorioPendenciaProfessor ?? throw new ArgumentNullException(nameof(repositorioPendenciaProfessor));
        }

        public async Task<long> Handle(ObterPendenciaPorTurmaCCPeriodoEscolarQuery request, CancellationToken cancellationToken)
            => await repositorioPendenciaProfessor.ObterPendenciaIdPorTurmaCCPeriodoEscolar(request.TurmaId, request.ComponenteCurricularId, request.PeriodoEscolarId, request.TipoPendencia);
    }
}
