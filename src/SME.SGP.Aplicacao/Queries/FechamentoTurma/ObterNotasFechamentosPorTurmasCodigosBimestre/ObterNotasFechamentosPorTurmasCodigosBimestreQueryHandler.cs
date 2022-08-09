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
    public class ObterNotasFechamentosPorTurmasCodigosBimestreQueryHandler :
        IRequestHandler<ObterNotasFechamentosPorTurmasCodigosBimestreQuery, IEnumerable<NotaConceitoBimestreComponenteDto>>
    {
        private readonly IRepositorioFechamentoNotaConsulta repositorioFechamentoNota;
        private readonly IMediator mediator;

        public ObterNotasFechamentosPorTurmasCodigosBimestreQueryHandler(IRepositorioFechamentoNotaConsulta repositorioFechamentoNota,
            IMediator mediator)
        {
            this.repositorioFechamentoNota = repositorioFechamentoNota ?? throw new ArgumentNullException(nameof(repositorioFechamentoNota));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<NotaConceitoBimestreComponenteDto>> Handle(ObterNotasFechamentosPorTurmasCodigosBimestreQuery request,
            CancellationToken cancellationToken)
        {
            var turmas = await mediator.Send(new ObterTurmasPorCodigosQuery(request.TurmasCodigos), cancellationToken);
            var turmasIds = turmas.Select(c => c.Id).ToArray();

            var notasFechamentos = (await mediator.Send(new ObterNotasFechamentosPorTurmasIdsQuery(turmasIds), cancellationToken))
                .Where(c => c.AlunoCodigo == request.AlunoCodigo);

            return notasFechamentos.Select(notaFechamento => new NotaConceitoBimestreComponenteDto
            {
                Id = 0,
                ConselhoClasseNotaId = 0,
                Bimestre = notaFechamento.Bimestre,
                Nota = notaFechamento.Nota,
                ConceitoId = notaFechamento.ConceitoId,
                TurmaCodigo = notaFechamento.TurmaCodigo,
                ComponenteCurricularCodigo = notaFechamento.ComponenteCurricularCodigo
            }).ToList();
        }
    }
}
