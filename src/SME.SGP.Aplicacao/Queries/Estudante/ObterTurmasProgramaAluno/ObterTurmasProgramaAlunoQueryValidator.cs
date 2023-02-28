using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasProgramaAlunoQueryValidator : AbstractValidator<ObterTurmasProgramaAlunoQuery>
    {
        public ObterTurmasProgramaAlunoQueryValidator()
        {
            RuleFor(x => x.CodigoAluno)
                .NotEmpty()
                .WithMessage("O código do aluno deve ser informado.");
        }
    }
}