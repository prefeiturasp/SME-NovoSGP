using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasAusenciaAvaliacaoPorPendenciaIdQueryHandler : IRequestHandler<ObterPendenciasAusenciaAvaliacaoPorPendenciaIdQuery, IEnumerable<PendenciaProfessorDto>>
    {
        private readonly IRepositorioPendenciaProfessor repositorioPendenciaProfessor;

        public ObterPendenciasAusenciaAvaliacaoPorPendenciaIdQueryHandler(IRepositorioPendenciaProfessor repositorioPendenciaProfessor)
        {
            this.repositorioPendenciaProfessor = repositorioPendenciaProfessor ?? throw new ArgumentNullException(nameof(repositorioPendenciaProfessor));
        }

        public async Task<IEnumerable<PendenciaProfessorDto>> Handle(ObterPendenciasAusenciaAvaliacaoPorPendenciaIdQuery request, CancellationToken cancellationToken)
            => await repositorioPendenciaProfessor.ObterPendenciasPorPendenciaId(request.PendenciaId);
    }
}
