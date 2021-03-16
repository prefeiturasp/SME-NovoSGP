using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoEscolarPorTurmaUseCase : AbstractUseCase, IObterPeriodoEscolarPorTurmaUseCase
    {
        public ObterPeriodoEscolarPorTurmaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<PeriodoEscolarPorTurmaDto>> Executar(long turmaId)
        {

            var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(turmaId));
            if (turma == null)
                throw new NegocioException($"Turma [{turmaId}] não localizada!");

            var periodos = await mediator.Send(new ObterPeriodosEscolaresPorAnoEModalidadeTurmaQuery(turma.ModalidadeCodigo, turma.AnoLetivo, turma.Semestre));
            
            return periodos?.Select(c => new PeriodoEscolarPorTurmaDto
            {
                Bimestre = c.Bimestre,
                Id = c.Id,
                Migrado = c.Migrado,
                PeriodoAberto = mediator.Send(new TurmaEmPeriodoAbertoQuery(turma, DateTime.Today, c.Bimestre, turma.AnoLetivo == DateTime.Today.Year)).Result,
        });
        }
    }
}
