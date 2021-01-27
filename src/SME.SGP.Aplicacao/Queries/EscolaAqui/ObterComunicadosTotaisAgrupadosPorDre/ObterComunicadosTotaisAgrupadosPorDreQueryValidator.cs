using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterComunicadosTotaisAgrupadosPorDreQueryValidator : AbstractValidator<ObterComunicadosTotaisAgrupadosPorDreQuery>
    {
        public ObterComunicadosTotaisAgrupadosPorDreQueryValidator()
        {
            RuleFor(x => x.AnoLetivo).NotEmpty().WithMessage("O ano letivo é obrigatório");
        }
    }
}