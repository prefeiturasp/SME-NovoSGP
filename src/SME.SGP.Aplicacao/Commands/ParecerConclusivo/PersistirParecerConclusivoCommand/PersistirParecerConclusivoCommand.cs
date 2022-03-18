using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class PersistirParecerConclusivoCommand : IRequest<bool>
    {
        public PersistirParecerConclusivoCommand(ConselhoClasseAluno conselhoClasseAluno)
        {
            ConselhoClasseAluno = conselhoClasseAluno;
        }

        public ConselhoClasseAluno ConselhoClasseAluno { get; }
    }

    public class PersistirParecerConclusivoCommandValidator : AbstractValidator<PersistirParecerConclusivoCommand>
    {
        public PersistirParecerConclusivoCommandValidator()
        {
            RuleFor(a => a.ConselhoClasseAluno)
                .NotEmpty()
                .WithMessage("O registro do conselho de classe do aluno deve ser informado para gerar seu parecer conclusivo");
        }
    }
}
