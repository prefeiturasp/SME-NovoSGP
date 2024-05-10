using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExistePlanejamentoAnualParaTurmaPeriodoEComponenteQuery : IRequest<long>
    {
        public ExistePlanejamentoAnualParaTurmaPeriodoEComponenteQuery(long turmaId, long periodoEscolarId, long componenteCurricularId)
        {
            TurmaId = turmaId;
            PeriodoEscolarId = periodoEscolarId;
            ComponenteCurricularId = componenteCurricularId;
        }

        public long TurmaId { get; set; }
        public long PeriodoEscolarId { get; set; }
        public long ComponenteCurricularId { get; set; }
    }

    public class ExistePlanejamentoAnualParaTurmaPeriodoEComponenteQueryValidator : AbstractValidator<ExistePlanejamentoAnualParaTurmaPeriodoEComponenteQuery>
    {
        public ExistePlanejamentoAnualParaTurmaPeriodoEComponenteQueryValidator()
        {
            RuleFor(c => c.TurmaId)
            .NotEmpty()
            .WithMessage("O id da turma deve ser informado para verificação da existencia de planejamento anual.");

            RuleFor(c => c.PeriodoEscolarId)
            .NotEmpty()
            .WithMessage("O id do periodo escolar deve ser informado para verificação da existencia de planejamento anual.");

            RuleFor(c => c.ComponenteCurricularId)
            .NotEmpty()
            .WithMessage("O componente curricular deve ser informado para verificação da existencia de planejamento anual.");

        }
    }
}
