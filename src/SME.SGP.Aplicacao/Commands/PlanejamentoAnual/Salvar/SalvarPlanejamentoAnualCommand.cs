using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class SalvarPlanejamentoAnualCommand : IRequest<PlanejamentoAnualAuditoriaDto>
    {
        public SalvarPlanejamentoAnualCommand()
        {

        }
        public SalvarPlanejamentoAnualCommand(long turmaId, long componenteCurricularId)
        {
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
        }
        public long TurmaId { get; set; }
        public long ComponenteCurricularId { get; set; }
        public IEnumerable<PlanejamentoAnualPeriodoEscolarDto> PeriodosEscolares { get; set; }
    }


    public class SalvarPlanejamentoAnualCommandValidator : AbstractValidator<SalvarPlanejamentoAnualCommand>
    {
        public SalvarPlanejamentoAnualCommandValidator()
        {

            RuleFor(c => c.PeriodosEscolares)
                .NotEmpty()
                .WithMessage("Os períodos escolares devem ser informados.");

            RuleFor(c => c.ComponenteCurricularId)
                .NotEmpty()
                .WithMessage("O componente curricular deve ser informado.");

            RuleFor(c => c.TurmaId)
                .NotEmpty()
                .WithMessage("A turma deve ser informada.");
        }
    }
}
