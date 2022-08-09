using FluentValidation;

namespace SME.SGP.Aplicacao;

public class ObterNotasFechamentosPorTurmasIdsQueryValidator : AbstractValidator<ObterNotasFechamentosPorTurmasIdsQuery>
{
    public ObterNotasFechamentosPorTurmasIdsQueryValidator()
    {
        RuleFor(c => c.TurmasIds)
            .NotNull()
            .WithMessage("Os Ids das turmas devem ser informados para a consulta das notas de fechamento.");
    }
}