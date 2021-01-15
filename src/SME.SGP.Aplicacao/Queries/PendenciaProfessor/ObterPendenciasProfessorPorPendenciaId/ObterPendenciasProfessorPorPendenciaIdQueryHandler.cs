using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasProfessorPorPendenciaIdQueryHandler : IRequestHandler<ObterPendenciasProfessorPorPendenciaIdQuery, IEnumerable<PendenciaProfessorDto>>
    {
        private readonly IRepositorioPendenciaProfessor repositorioPendenciaProfessor;

        public ObterPendenciasProfessorPorPendenciaIdQueryHandler(IRepositorioPendenciaProfessor repositorioPendenciaProfessor)
        {
            this.repositorioPendenciaProfessor = repositorioPendenciaProfessor ?? throw new ArgumentNullException(nameof(repositorioPendenciaProfessor));
        }

        public async Task<IEnumerable<PendenciaProfessorDto>> Handle(ObterPendenciasProfessorPorPendenciaIdQuery request, CancellationToken cancellationToken)
            => await repositorioPendenciaProfessor.ObterPendenciasPorPendenciaId(request.PendenciaId);
    }
}
