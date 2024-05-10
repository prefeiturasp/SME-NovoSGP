using MediatR;
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
        private readonly IMediator mediator;

        public ObterNotasFinaisBimestresAlunoQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<NotaConceitoBimestreComponenteDto>> Handle(ObterNotasFinaisBimestresAlunoQuery request, CancellationToken cancellationToken)
        {
            var notasConceitosFechamento = await mediator.Send(new ObterNotasConceitosFechamentoPorTurmasCodigosEBimestreQuery(request.TurmasCodigos, request.Bimestre, tipoCalendario:request.TipoCalendario, alunoCodigo: request.AlunoCodigo), cancellationToken);

            var notasConceitosConselhoClasse = await mediator.Send(new ObterNotasConceitosConselhoClassePorTurmasCodigosEBimestreQuery(request.TurmasCodigos, request.Bimestre, tipoCalendario: request.TipoCalendario, alunoCodigo: request.AlunoCodigo), cancellationToken);
            notasConceitosConselhoClasse = notasConceitosConselhoClasse.Where(c => c.NotaConceito.HasValue);
                 
            var notasFinais = new List<NotaConceitoBimestreComponenteDto>();
            
            notasFinais.AddRange(notasConceitosConselhoClasse);
            notasFinais.AddRange(notasConceitosFechamento.Where(fechamento => 
                !notasConceitosConselhoClasse.Any(conselho => conselho.ComponenteCurricularCodigo == fechamento.ComponenteCurricularCodigo && conselho.Bimestre == fechamento.Bimestre)));
            return notasFinais;
        }
    }
}
