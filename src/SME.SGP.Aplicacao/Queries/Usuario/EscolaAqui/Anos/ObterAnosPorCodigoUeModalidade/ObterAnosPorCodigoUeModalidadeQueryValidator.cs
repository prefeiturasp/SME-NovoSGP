using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterAnosPorCodigoUeModalidadeQueryValidator : AbstractValidator<ObterAnosPorCodigoUeModalidadeQuery>
    {
        public ObterAnosPorCodigoUeModalidadeQueryValidator()
        {
            RuleFor(x => x.Modalidade).NotEmpty().WithMessage("A Modalidade é Obrigatória");
        }
    }
}
