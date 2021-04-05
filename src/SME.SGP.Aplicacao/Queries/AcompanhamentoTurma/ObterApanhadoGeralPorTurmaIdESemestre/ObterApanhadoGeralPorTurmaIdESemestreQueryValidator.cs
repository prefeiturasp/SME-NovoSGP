using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterApanhadoGeralPorTurmaIdESemestreQueryValidator : AbstractValidator<ObterApanhadoGeralPorTurmaIdESemestreQuery>
    {
        public ObterApanhadoGeralPorTurmaIdESemestreQueryValidator()
        {
            RuleFor(a => a.TurmaId)
                .NotEmpty()
                .WithMessage("A turma ID deve ser informada");

            RuleFor(a => a.Semestre)
                .NotEmpty()
                .WithMessage("O Semestre deve ser informado");
        }
    }
}