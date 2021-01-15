using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterComunicadosTotaisQueryValidator : AbstractValidator<ObterComunicadosTotaisQuery>
    {
        public ObterComunicadosTotaisQueryValidator()
        {
            RuleFor(x => x.AnoLetivo).NotEmpty().WithMessage("O ano letivo é obrigatório");
        }
    }
}