using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarFechamentoConsolidadoPorTurmaBimestreComponenteUseCase : AbstractUseCase, IExecutarFechamentoConsolidadoPorTurmaBimestreComponenteUseCase
    {
        public ExecutarFechamentoConsolidadoPorTurmaBimestreComponenteUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var command = mensagemRabbit.ObterObjetoMensagem<FechamentoConsolidacaoTurmaComponenteBimestreDto>();

            var fechamentos = await mediator.Send(new ObterFechamentosTurmaComponentesQuery(command.TurmaId, new long[] { command.ComponenteCurricularId }, command.Bimestre));

            var professoresDaTurma = await mediator.Send(new ObterProfessoresTitularesPorTurmaIdQuery(command.TurmaId));

            var lstConsolidado = MapearFechamentoConsolidado(fechamentos, professoresDaTurma);

            foreach (var consolidado in lstConsolidado)
            {
                await mediator.Send(new SalvarFechamentoConsolidadoCommand(consolidado));
            }

            return true;
        }

        private IEnumerable<FechamentoConsolidadoComponenteTurma> MapearFechamentoConsolidado(IEnumerable<FechamentoTurmaDisciplina> fechamentos, IEnumerable<Infra.ProfessorTitularDisciplinaEol> professoresDaTurma)
        {
            foreach (var fechamento in fechamentos)
            {
                var professorComponente = professoresDaTurma.FirstOrDefault(p => p.DisciplinaId == fechamento.DisciplinaId);

                yield return new FechamentoConsolidadoComponenteTurma()
                {
                    Bimestre = fechamento.FechamentoTurma.PeriodoEscolar.Bimestre,
                    ComponenteCurricularCodigo = fechamento.DisciplinaId,
                    DataAtualizacao = DateTime.Now,
                    TurmaId = fechamento.FechamentoTurma.TurmaId,
                    Status = fechamento.ObterStatusFechamento(),
                    ProfessorNome = professorComponente.ProfessorNome,
                    ProfessorRf = professorComponente.ProfessorRf
                };
            }
        }
    }
}
