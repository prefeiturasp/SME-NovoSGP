using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class NotificarPeriodoFechamentoUeCommand : IRequest<bool>
    {
        public NotificarPeriodoFechamentoUeCommand(ModalidadeTipoCalendario modalidadeTipoCalendario, PeriodoFechamentoBimestre periodoFechamentoBimestre)
        {
            PeriodoFechamentoBimestre = periodoFechamentoBimestre;
            ModalidadeTipoCalendario = modalidadeTipoCalendario;
        }

        public ModalidadeTipoCalendario ModalidadeTipoCalendario { get; set; }
        public PeriodoFechamentoBimestre PeriodoFechamentoBimestre { get; set; }
    }

    public class NotificarPeriodoFechamentoUeCommandValidator : AbstractValidator<NotificarPeriodoFechamentoUeCommand>
    {
        public NotificarPeriodoFechamentoUeCommandValidator()
        {
            RuleFor(c => c.PeriodoFechamentoBimestre)
               .NotEmpty()
               .WithMessage("O periodo de fechamento deve ser informado para Notificação das UEs.");

        }
    }
}
