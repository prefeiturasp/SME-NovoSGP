using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Aplicacao
{
    public class ExecutaNotificacaoUeFechamentoInsuficientesCommand : IRequest<bool>
    {
        public ExecutaNotificacaoUeFechamentoInsuficientesCommand(IGrouping<long, PeriodoFechamentoBimestre> periodosEncerrando, ModalidadeTipoCalendario modalidade, double percentualFechamentoInsuficiente)
        {
            PeriodosEncerrando = periodosEncerrando;
            Modalidade = modalidade;
            PercentualFechamentoInsuficiente = percentualFechamentoInsuficiente;
        }

        public IGrouping<long, PeriodoFechamentoBimestre> PeriodosEncerrando { get; set; }
        public ModalidadeTipoCalendario Modalidade { get; set; }
        public double PercentualFechamentoInsuficiente { get; set; }
    }

    public class ExecutaNotificacaoUeFechamentoInsuficientesCommandValidator : AbstractValidator<ExecutaNotificacaoUeFechamentoInsuficientesCommand>
    {
        public ExecutaNotificacaoUeFechamentoInsuficientesCommandValidator()
        {
            RuleFor(c => c.PeriodosEncerrando)
               .NotEmpty()
               .WithMessage("O periodo de fechamento que esta encerrando deve ser informado para notifação da UE.");
        }
    }
}
