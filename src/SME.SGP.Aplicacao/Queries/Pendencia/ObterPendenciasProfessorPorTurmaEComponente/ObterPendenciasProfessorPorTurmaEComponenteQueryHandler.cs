using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasProfessorPorTurmaEComponenteQueryHandler : IRequestHandler<ObterPendenciasProfessorPorTurmaEComponenteQuery, IEnumerable<PendenciaProfessor>>
    {
        private readonly IRepositorioPendenciaProfessor repositorioPendenciaProfessor;

        public ObterPendenciasProfessorPorTurmaEComponenteQueryHandler(IRepositorioPendenciaProfessor repositorioPendenciaProfessor) 
        {
            this.repositorioPendenciaProfessor = repositorioPendenciaProfessor ?? throw new ArgumentNullException(nameof(repositorioPendenciaProfessor));
        }

        public async Task<IEnumerable<PendenciaProfessor>> Handle(ObterPendenciasProfessorPorTurmaEComponenteQuery request, CancellationToken cancellationToken)
            => await repositorioPendenciaProfessor.ObterPendenciasProfessorPorTurmaEComponente(request.TurmaId, request.ComponentesCurriculares, request.PeriodoEscolarId, request.TipoPendencia);
    }
}
