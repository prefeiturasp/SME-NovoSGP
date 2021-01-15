using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarPendenciaProfessorCommandHandler : IRequestHandler<SalvarPendenciaProfessorCommand, long>
    {
        private readonly IRepositorioPendenciaProfessor repositorioPendenciaProfessor;

        public SalvarPendenciaProfessorCommandHandler(IRepositorioPendenciaProfessor repositorioPendenciaProfessor)
        {
            this.repositorioPendenciaProfessor = repositorioPendenciaProfessor ?? throw new ArgumentNullException(nameof(repositorioPendenciaProfessor));
        }

        public async Task<long> Handle(SalvarPendenciaProfessorCommand request, CancellationToken cancellationToken)
            => await repositorioPendenciaProfessor.Inserir(request.PendenciaId, request.TurmaId, request.ComponenteCurricularId, request.ProfessorRf, request.PeriodoEscolarId);
    }
}
