using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterTipoRelatorioPorCodigoQueryValidator : AbstractValidator<ObterTipoRelatorioPorCodigoQuery>
    {
        public ObterTipoRelatorioPorCodigoQueryValidator()
        {
            RuleFor(c => c.CodigoRelatorio)
                .NotEmpty()
                .NotNull()
                .WithMessage("O código do relatório deve ser informado para obter o tipo do relatório.");
        }
    }
}