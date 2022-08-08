using FluentValidation;

namespace SME.SGP.Aplicacao;

public class ObterConselhoDeClasseNotaBimestresCacheQueryValidator : AbstractValidator<ObterConselhoDeClasseNotaBimestresCacheQuery>
{
    public ObterConselhoDeClasseNotaBimestresCacheQueryValidator()
    {
        RuleFor(c => c.ConselhoClasseId)
            .GreaterThan(0)
            .WithMessage("O Id do conselho de classe deve ser informado para a consulta do conselho de classe.");

        RuleFor(c => c.CodigoAluno)
            .NotEmpty()
            .NotNull()
            .WithMessage("O código do aluno deve ser informado para a consulta do conselho de classe.");

        RuleFor(c => c.Bimestre)
            .GreaterThanOrEqualTo(0)
            .WithMessage("O bimestre deve ser informado para a consulta do conselho de classe.");
    }
}