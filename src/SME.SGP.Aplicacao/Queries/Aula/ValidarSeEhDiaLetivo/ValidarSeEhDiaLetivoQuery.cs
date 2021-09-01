using FluentValidation;
using MediatR;
using System;

namespace SME.SGP.Aplicacao
{
    public class ValidarSeEhDiaLetivoQuery : IRequest<bool>
    {
        public ValidarSeEhDiaLetivoQuery(DateTime dataInicio, long tipoCalendarioId, string ueId)
        {
            DataInicio = dataInicio;
            TipoCalendarioId = tipoCalendarioId;
            UeId = ueId;
        }

        public DateTime DataInicio { get; }
        public long TipoCalendarioId { get; }
        public string UeId { get; }
    }

    public class ValidarSeEhDiaLetivoQueryValidator : AbstractValidator<ValidarSeEhDiaLetivoQuery>
    {
        public ValidarSeEhDiaLetivoQueryValidator()
        {
            RuleFor(a => a.DataInicio)
                .NotEmpty()
                .WithMessage("A data de inicício deve ser informada para validação de dia letivo");

            RuleFor(a => a.TipoCalendarioId)
                .NotEmpty()
                .WithMessage("O tipo de calendário deve ser informado para validação de dia letivo");
        }
    }
}
