using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciaAulaPorTurmaIdDisciplinaIdQueryHandler : IRequestHandler<ObterPendenciaAulaPorTurmaIdDisciplinaIdQuery, long>
    {
        private readonly IRepositorioPendenciaAula repositorioPendenciaAula;

        public ObterPendenciaAulaPorTurmaIdDisciplinaIdQueryHandler(IRepositorioPendenciaAula repositorioPendenciaAula)
        {
            this.repositorioPendenciaAula = repositorioPendenciaAula ?? throw new ArgumentNullException(nameof(repositorioPendenciaAula));
        }

        public async Task<long> Handle(ObterPendenciaAulaPorTurmaIdDisciplinaIdQuery request, CancellationToken cancellationToken)
                => await repositorioPendenciaAula.ObterPendenciaAulaPorTurmaIdDisciplinaId(request.TurmaId, request.DisciplinaId, request.ProfessorRf, request.TipoPendencia);
    }
}
