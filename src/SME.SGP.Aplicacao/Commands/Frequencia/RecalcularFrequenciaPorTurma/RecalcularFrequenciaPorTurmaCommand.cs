using FluentValidation;
using MediatR;
using System;

namespace SME.SGP.Aplicacao
{
    public class RecalcularFrequenciaPorTurmaCommand : IRequest<bool>
    {
        public RecalcularFrequenciaPorTurmaCommand(string turmaCodigo, string componenteCurricularId, long aulaId)
        {
            TurmaCodigo = turmaCodigo;
            ComponenteCurricularId = componenteCurricularId;
            AulaId = aulaId;
        }

        public string TurmaCodigo { get; }
        public string ComponenteCurricularId { get; }
        public long AulaId { get; }
    }

    public class RecalcularFrequenciaPorTurmaCommandValidator : AbstractValidator<RecalcularFrequenciaPorTurmaCommand>
    {
        public RecalcularFrequenciaPorTurmaCommandValidator()
        {
            RuleFor(a => a.TurmaCodigo)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado para recalculo da frequencia da turma.");

            RuleFor(a => a.ComponenteCurricularId)
                .NotEmpty()
                .WithMessage("O código do componente curricular deve ser informado para recalculo da frequencia da turma.");

            RuleFor(a => a.AulaId)
                .NotEmpty()
                .WithMessage("O identificador da aula deve ser informado para recalculo da frequencia da turma.");
        }
    }
}
