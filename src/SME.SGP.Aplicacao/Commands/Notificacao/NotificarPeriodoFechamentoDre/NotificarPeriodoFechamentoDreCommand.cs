using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class NotificarPeriodoFechamentoDreCommand : IRequest<bool>
    {
        public NotificarPeriodoFechamentoDreCommand(ModalidadeTipoCalendario modalidadeTipoCalendario, PeriodoFechamentoBimestre periodoFechamentoBimestre)
        {
            PeriodoFechamentoBimestre = periodoFechamentoBimestre;
            ModalidadeTipoCalendario = modalidadeTipoCalendario;
        }

        public ModalidadeTipoCalendario ModalidadeTipoCalendario { get; set; }
        public PeriodoFechamentoBimestre PeriodoFechamentoBimestre { get; set; }
    }

    public class NotificarPeriodoFechamentoDreCommandValidator : AbstractValidator<NotificarPeriodoFechamentoDreCommand>
    {
        public NotificarPeriodoFechamentoDreCommandValidator()
        {
            RuleFor(c => c.PeriodoFechamentoBimestre)
               .NotEmpty()
               .WithMessage("O periodo de fechamento deve ser informado para Notificação das UEs.");

        }
    }
}
