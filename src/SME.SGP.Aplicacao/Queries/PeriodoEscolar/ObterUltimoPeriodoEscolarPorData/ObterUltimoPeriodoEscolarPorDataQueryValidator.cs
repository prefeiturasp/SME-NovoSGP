using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterUltimoPeriodoEscolarPorDataQueryValidator : AbstractValidator<ObterUltimoPeriodoEscolarPorDataQuery>
    {
        public ObterUltimoPeriodoEscolarPorDataQueryValidator()
        {
            RuleFor(a => a.AnoLetivo)
                .NotEmpty()
                .WithMessage("O Ano Letivo deve ser informado.");

            RuleFor(a => a.ModalidadeTipoCalendario)
                .NotEmpty()
                .WithMessage("A modalidade deve ser informada.");

            RuleFor(a => a.DataAtual)
                .NotEmpty()
                .WithMessage("Data da atual deve ser informada");
        }
    }
}
