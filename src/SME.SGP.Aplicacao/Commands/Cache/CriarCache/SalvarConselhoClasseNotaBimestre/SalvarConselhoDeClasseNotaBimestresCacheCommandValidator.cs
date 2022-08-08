using FluentValidation;

namespace SME.SGP.Aplicacao;

public class SalvarConselhoDeClasseNotaBimestresCacheCommandValidator : AbstractValidator<SalvarConselhoDeClasseNotaBimestresCacheCommand>
{
    public SalvarConselhoDeClasseNotaBimestresCacheCommandValidator()
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
            .WithMessage("O bimestre deve ser informado para salvar o conselho de classe.");

        RuleFor(c => c.ConselhoClasseNotaDto)
            .NotNull()
            .WithMessage("Os dados da nota deve ser informado para salvar o conselho de classe.");
    }
}