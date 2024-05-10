using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterNecessidadesEspeciaisAlunoQueryValidator : AbstractValidator<ObterNecessidadesEspeciaisAlunoQuery>
    {
        public ObterNecessidadesEspeciaisAlunoQueryValidator()
        {
            RuleFor(x => x.CodigoAluno)
                .NotEmpty()
                .WithMessage("O código do aluno deve ser informado.");
        }
    }
}