using FluentValidation;

namespace SME.SGP.Aplicacao.Queries.Frequencia.ObterFrequenciaPorLimitePercentual
{
    public class ObterQuantidadeAbaixoFrequenciaPorTurmaQueryValidator : AbstractValidator<ObterQuantidadeAbaixoFrequenciaPorTurmaQuery>
    {
        public ObterQuantidadeAbaixoFrequenciaPorTurmaQueryValidator()
        {
            RuleFor(c => c.AnoLetivo)
                .NotNull()
                .NotEmpty()
                .WithMessage("O ano letivo precisa ser informado");
        }
    }
}
