using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExistePendenciaProfessorPorTurmaEComponenteQueryHandler : IRequestHandler<ExistePendenciaProfessorPorTurmaEComponenteQuery, bool>
    {
        private readonly IRepositorioPendenciaProfessor repositorioPendenciaProfessor;

        public ExistePendenciaProfessorPorTurmaEComponenteQueryHandler(IRepositorioPendenciaProfessor repositorioPendenciaProfessor)
        {
            this.repositorioPendenciaProfessor = repositorioPendenciaProfessor ?? throw new ArgumentNullException(nameof(repositorioPendenciaProfessor));
        }

        public async Task<bool> Handle(ExistePendenciaProfessorPorTurmaEComponenteQuery request, CancellationToken cancellationToken)
            => await repositorioPendenciaProfessor.ExistePendenciaProfessorPorTurmaEComponente(request.TurmaId, request.ComponenteCurricularId, request.PeridoEscolarId, request.ProfessorRf, request.TipoPendencia);
    }
}
