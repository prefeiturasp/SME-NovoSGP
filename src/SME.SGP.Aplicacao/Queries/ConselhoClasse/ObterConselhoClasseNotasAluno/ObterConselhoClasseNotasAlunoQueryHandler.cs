using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterConselhoClasseNotasAlunoQueryHandler : IRequestHandler<ObterConselhoClasseNotasAlunoQuery, IEnumerable<NotaConceitoBimestreComponenteDto>>
    {
        private readonly IMediator mediator;

        public ObterConselhoClasseNotasAlunoQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<NotaConceitoBimestreComponenteDto>> Handle(ObterConselhoClasseNotasAlunoQuery request, CancellationToken cancellationToken)
        {
            var turmaCodigo = await mediator.Send(new ObterTurmaPorConselhoClasseIdQuery(request.ConselhoClasseId), cancellationToken); 
            var turmas = await mediator.Send(new ObterTurmasPorCodigosQuery(new[] { turmaCodigo }), cancellationToken);

            var turmasIds = turmas.Select(c => c.Id).ToArray();
            
            var notasConceitosConselhoClasse = (await mediator.Send(new ObterNotasConceitosConselhoClassePorTurmaIdEBimestreQuery(turmasIds, request.Bimestre), cancellationToken))
                .Where(c => c.AlunoCodigo == request.AlunoCodigo && c.ConselhoClasseId == request.ConselhoClasseId);

            var notasConceitos = new List<NotaConceitoComponenteBimestreAlunoDto>();
            notasConceitos.AddRange(notasConceitosConselhoClasse);
            
            return notasConceitos.Select(notaConceito => new NotaConceitoBimestreComponenteDto
            {
                Id = notaConceito.ConselhoClasseId,
                ConselhoClasseNotaId = notaConceito.ConselhoClasseNotaId,
                Bimestre = notaConceito.Bimestre,
                Nota = notaConceito.Nota,
                ConceitoId = notaConceito.ConceitoId,
                TurmaCodigo = notaConceito.TurmaCodigo,
                ComponenteCurricularCodigo = notaConceito.ComponenteCurricularCodigo
            }).ToList();            
        }
    }
}