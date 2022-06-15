using FluentValidation;
using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class FiltroRelatorioFrequenciaMensalDtoValidator : AbstractValidator<FiltroRelatorioFrequenciaMensalDto>
    {
        public FiltroRelatorioFrequenciaMensalDtoValidator()
        {
            RuleFor(c => c.AnoLetivo)
                .GreaterThanOrEqualTo(2020)
                .WithMessage("O ano letivo deve ser informado e ser maior ou igual ao ano de 2020.");

            RuleFor(c => c.Modalidade)
                .IsInEnum()
                .WithMessage("A modalidade deve ser informada.");

            RuleFor(c => c.Semestre)
                .NotEmpty()
                .WithMessage("Quando a modalidade é EJA o Semestre deve ser informado.")
                .When(c => c.Modalidade == Modalidade.EJA);

            RuleFor(c => c.TipoFormatoRelatorio)
                .NotEmpty()
                .WithMessage("O formato deve ser informado.");
        }
    }
}
