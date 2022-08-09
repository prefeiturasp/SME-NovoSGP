using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao 
{
  public  class ObterNotasFinaisBimestresAlunoQueryHandler : IRequestHandler<ObterNotasFinaisBimestresAlunoQuery, IEnumerable<NotaConceitoBimestreComponenteDto>>
    {
        private readonly IRepositorioConselhoClasseNotaConsulta repositorioConselhoClasseNota;
        private readonly IMediator mediator;

        public ObterNotasFinaisBimestresAlunoQueryHandler(IRepositorioConselhoClasseNotaConsulta repositorioConselhoClasseNota,
            IMediator mediator)
        {
            this.repositorioConselhoClasseNota = repositorioConselhoClasseNota ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseNota));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<NotaConceitoBimestreComponenteDto>> Handle(ObterNotasFinaisBimestresAlunoQuery request, CancellationToken cancellationToken)
        {
            var turmas = await mediator.Send(new ObterTurmasPorCodigosQuery(request.TurmasCodigos), cancellationToken);
            var turmasIds = turmas.Select(c => c.Id).ToArray();

            var notasConceitosFechamento = (await mediator.Send(new ObterNotasConceitosFechamentoPorTurmaIdEBimestreQuery(turmasIds, request.Bimestre), cancellationToken))
                .Where(c => c.AlunoCodigo == request.AlunoCodigo);
            
            var notasConceitosConselhoClasse = (await mediator.Send(new ObterNotasConceitosConselhoClassePorTurmaIdEBimestreQuery(turmasIds, request.Bimestre), cancellationToken))
                .Where(c => c.AlunoCodigo == request.AlunoCodigo);

            var notasFinais = new List<NotaConceitoComponenteBimestreAlunoDto>();
            notasFinais.AddRange(notasConceitosFechamento);
            notasFinais.AddRange(notasConceitosConselhoClasse);
            
            return notasFinais.Select(notaFinal => new NotaConceitoBimestreComponenteDto
            {
                Id = notaFinal.ConselhoClasseId,
                ConselhoClasseNotaId = notaFinal.ConselhoClasseNotaId,
                Bimestre = notaFinal.Bimestre,
                Nota = notaFinal.Nota,
                ConceitoId = notaFinal.ConceitoId,
                TurmaCodigo = notaFinal.TurmaCodigo,
                ComponenteCurricularCodigo = notaFinal.ComponenteCurricularCodigo
            }).ToList();
        }
    }
}
