using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterBimestreAtualPorTurmaIdUseCase : IObterBimestreAtualPorTurmaIdUseCase
    {
        private readonly IMediator mediator;

        public ObterBimestreAtualPorTurmaIdUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<BimestreDto> Executar(long turmaId)
        {
            var turma = await mediator.Send(new ObterTurmaPorIdQuery(turmaId));

            if (turma == null)
                throw new NegocioException("A turma informada não foi encontrada");

            var periodos = await mediator.Send(new ObterPeriodosEscolaresPorAnoEModalidadeTurmaQuery(turma.ModalidadeCodigo, turma.AnoLetivo, turma.Semestre));

            var periodoEscolar = await mediator.Send(new ObterPeriodoEscolarAtualQuery(turmaId, DateTime.Now.Date));

            long idBimestre = 0;
            if (periodos.Any() && periodoEscolar != null)
                idBimestre = periodos.FirstOrDefault(x => x.Bimestre == periodoEscolar.Bimestre)?.Id ?? 0;

            return periodoEscolar == null || idBimestre == 0 ? null :
                new BimestreDto() { Id = idBimestre, Numero = periodoEscolar.Bimestre };
        }
    }
}
