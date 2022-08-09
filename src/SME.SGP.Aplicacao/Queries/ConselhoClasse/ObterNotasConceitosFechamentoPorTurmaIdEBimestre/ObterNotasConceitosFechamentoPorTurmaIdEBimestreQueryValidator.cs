using FluentValidation;

namespace SME.SGP.Aplicacao;

public class ObterNotasConceitosFechamentoPorTurmaIdEBimestreQueryValidator : AbstractValidator<ObterNotasConceitosFechamentoPorTurmaIdEBimestreQuery>
{
    public ObterNotasConceitosFechamentoPorTurmaIdEBimestreQueryValidator()
    {
        RuleFor(c => c.TurmasIds)
            .NotNull()
            .WithMessage("Os Ids das turmas precisam ser informados para consultar as notas do fechamento do bimestre.");

        RuleFor(c => c.Bimestre)
            .GreaterThanOrEqualTo(0)
            .WithMessage(
                "O bimestre deve ser informado com um valor maior ou igual a 0(zero) para consultar as notas do fechamento do bimestre.");        
    }
}