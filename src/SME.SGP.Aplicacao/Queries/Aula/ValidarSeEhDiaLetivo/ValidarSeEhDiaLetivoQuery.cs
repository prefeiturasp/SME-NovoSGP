using FluentValidation;
using MediatR;
using System;

namespace SME.SGP.Aplicacao
{
    public class ValidarSeEhDiaLetivoQuery : IRequest<bool>
    {
        public ValidarSeEhDiaLetivoQuery(DateTime dataInicio, DateTime dataFim, long tipoCalendarioId, bool ehLetivo = false, long tipoEventoId = 0)
        {
            DataInicio = dataInicio;
            DataFim = dataFim;
            TipoCalendarioId = tipoCalendarioId;
            EhLetivo = ehLetivo;
            TipoEventoId = tipoEventoId;
        }

        public DateTime DataInicio { get; }
        public DateTime DataFim { get; }
        public long TipoCalendarioId { get; }
        public bool EhLetivo { get; }
        public long TipoEventoId { get; }
    }

    public class ValidarSeEhDiaLetivoQueryValidator : AbstractValidator<ValidarSeEhDiaLetivoQuery>
    {
        public ValidarSeEhDiaLetivoQueryValidator()
        {
            RuleFor(a => a.DataInicio)
                .NotEmpty()
                .WithMessage("A data de inicício deve ser informada para validação de dia letivo");

            RuleFor(a => a.DataFim)
                .NotEmpty()
                .WithMessage("A data de fim deve ser informada para validação de dia letivo");

            RuleFor(a => a.TipoCalendarioId)
                .NotEmpty()
                .WithMessage("O tipo de calendário deve ser informado para validação de dia letivo");
        }
    }
}
