using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciaIdPorTurmaQueryHandler : IRequestHandler<ObterPendenciaIdPorTurmaQuery, long>
    {
        private readonly IRepositorioPendenciaProfessor repositorioPendenciaProfessor;

        public ObterPendenciaIdPorTurmaQueryHandler(IRepositorioPendenciaProfessor repositorioPendenciaProfessor)
        {
            this.repositorioPendenciaProfessor = repositorioPendenciaProfessor ?? throw new ArgumentNullException(nameof(repositorioPendenciaProfessor));
        }

        public async Task<long> Handle(ObterPendenciaIdPorTurmaQuery request, CancellationToken cancellationToken)
            => await repositorioPendenciaProfessor.ObterPendenciaIdPorTurma(request.TurmaId, request.TipoPendencia);
    }
}
