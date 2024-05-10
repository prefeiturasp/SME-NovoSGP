using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class NotificaFrequenciaPeriodoUeCommand : IRequest<bool>
    {
        public NotificaFrequenciaPeriodoUeCommand(PeriodoEscolar periodoEscolarEncerrado)
        {
            PeriodoEscolarEncerrado = periodoEscolarEncerrado;
        }

        public PeriodoEscolar PeriodoEscolarEncerrado { get; set; }
    }

    public class NotificaFrequenciaPeriodoUeCommandValidator : AbstractValidator<NotificaFrequenciaPeriodoUeCommand>
    {
        public NotificaFrequenciaPeriodoUeCommandValidator()
        {
            RuleFor(c => c.PeriodoEscolarEncerrado)
               .NotEmpty()
               .WithMessage("O periodo escolar encerrado deve ser informado para notificação da UE.");

        }
    }
}
