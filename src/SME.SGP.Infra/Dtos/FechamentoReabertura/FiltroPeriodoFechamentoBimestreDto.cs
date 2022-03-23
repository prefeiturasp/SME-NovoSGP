using FluentValidation;
using SME.SGP.Dominio;


namespace SME.SGP.Infra.Dtos
{
    public class FiltroPeriodoFechamentoBimestreDto
    {
        public FiltroPeriodoFechamentoBimestreDto(PeriodoFechamentoBimestre periodoFechamentoBimestre, ModalidadeTipoCalendario modalidadeTipoCalendario)
        {
            PeriodoFechamentoBimestre = periodoFechamentoBimestre;
            ModalidadeTipoCalendario = modalidadeTipoCalendario;
        }

        public PeriodoFechamentoBimestre PeriodoFechamentoBimestre { get; set; }
        public ModalidadeTipoCalendario ModalidadeTipoCalendario;
    }

    public class FiltroPeriodoFechamentoBimestreDtoValidator : AbstractValidator<FiltroPeriodoFechamentoBimestreDto>
    {
        public FiltroPeriodoFechamentoBimestreDtoValidator()
        {
            RuleFor(c => c.PeriodoFechamentoBimestre)
                .NotEmpty()
                .WithMessage("O período de fechamento bimestre deve ser informada.");

            RuleFor(c => c.ModalidadeTipoCalendario)
                .NotEmpty()
                .WithMessage("A Modalidade Tipo Calendario deve ser informada.");
        }
    }
}
