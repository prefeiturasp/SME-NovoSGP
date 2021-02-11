using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class SalvarPlanoAeeCommandValidator : AbstractValidator<SalvarPlanoAeeCommand>
    {
        public SalvarPlanoAeeCommandValidator()
        {
            RuleFor(x => x.TurmaId)
                   .GreaterThan(0)
                   .WithMessage("A turma deve ser informada!");

            RuleFor(x => x.AlunoCodigo)
                    .NotEmpty()
                    .WithMessage("O código do estudante deve ser informado!");

            RuleFor(x => x.AlunoNome)
                    .NotEmpty()
                    .WithMessage("O nome do estudante deve ser informado!");
        }
    }
}
