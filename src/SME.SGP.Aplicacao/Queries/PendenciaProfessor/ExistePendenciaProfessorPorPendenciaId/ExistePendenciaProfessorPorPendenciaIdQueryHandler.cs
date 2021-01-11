using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExistePendenciaProfessorPorPendenciaIdQueryHandler : IRequestHandler<ExistePendenciaProfessorPorPendenciaIdQuery, bool>
    {
        private readonly IRepositorioPendenciaProfessor repositorioPendenciaProfessor;

        public ExistePendenciaProfessorPorPendenciaIdQueryHandler(IRepositorioPendenciaProfessor repositorioPendenciaProfessor)
        {
            this.repositorioPendenciaProfessor = repositorioPendenciaProfessor ?? throw new ArgumentNullException(nameof(repositorioPendenciaProfessor));
        }

        public async Task<bool> Handle(ExistePendenciaProfessorPorPendenciaIdQuery request, CancellationToken cancellationToken)
            => await repositorioPendenciaProfessor.ExistePendenciaProfessorPorPendenciaId(request.PendenciaId);
    }
}
