using FluentValidation;

namespace SME.SGP.Aplicacao.Queries.Abrangencia.ObterTurmaPorAnoLetivoCodigoUeModalidade
{
    public class ObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreAnosQueryValidator : AbstractValidator<ObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreAnosQuery>
    {
        public ObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreAnosQueryValidator()
        {
            RuleFor(x => x.AnoLetivo).GreaterThan(1900).WithMessage("O Ano letivo deve ser informado");
            RuleFor(x => x.CodigoUe).NotEmpty().WithMessage("O Codigo da Ue deve ser informado");
        }
    }
}

