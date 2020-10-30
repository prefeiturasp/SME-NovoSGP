using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterComunicadosPaginadosQueryValidator : AbstractValidator<ObterComunicadosPaginadosQuery>
    {
        public ObterComunicadosPaginadosQueryValidator()
        {
            RuleFor(x => x.AnoLetivo).NotEmpty().WithMessage("O Ano letivo é Obrigatório");
            RuleFor(x => x.CodigoDre).NotEmpty().WithMessage("O Codigo da Dre é Obrigatório");
            RuleFor(x => x.CodigoUe).NotEmpty().WithMessage("O Codigo da Ue é Obrigatório");
        }
    }
}
