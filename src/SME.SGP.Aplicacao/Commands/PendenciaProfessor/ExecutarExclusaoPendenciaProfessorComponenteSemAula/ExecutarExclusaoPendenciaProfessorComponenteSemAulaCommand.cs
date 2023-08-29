using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class ExecutarExclusaoPendenciaProfessorComponenteSemAulaCommand : IRequest<bool>
    {
        public ExecutarExclusaoPendenciaProfessorComponenteSemAulaCommand(Turma turma, long componenteCurricularId, DateTime dataAula)
        {
            Turma = turma;
            ComponenteCurricularId = componenteCurricularId;
            DataAula = dataAula;
        }

        public Turma Turma { get; set; }
        public long ComponenteCurricularId { get; set; }
        public DateTime DataAula { get; set; }
    }

    public class ExecutarExclusaoPendenciaProfessorComponenteSemAulaCommandValidator : AbstractValidator<ExecutarExclusaoPendenciaProfessorComponenteSemAulaCommand>
    {
        public ExecutarExclusaoPendenciaProfessorComponenteSemAulaCommandValidator()
        {
            RuleFor(c => c.Turma)
               .NotEmpty()
               .WithMessage("A turma deve ser informada para sua exclusão da pendência do professor.");

            RuleFor(c => c.ComponenteCurricularId)
               .NotEmpty()
               .WithMessage("O id do componente curricular deve ser informada para sua exclusão da pendência do professor.");

            RuleFor(c => c.DataAula)
               .NotEmpty()
               .WithMessage("A data da aula deve ser informada para sua exclusão da pendência do professor.");
        }
    }
}
