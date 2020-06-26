using FluentValidation;
using MediatR;
using System;

namespace SME.SGP.Aplicacao.Queries.Evento.ObterDataPossuiEventoLiberacaoExcepcional
{
    public class ObterDataPossuiEventoLiberacaoExcepcionalQuery : IRequest<bool>
    {
        public ObterDataPossuiEventoLiberacaoExcepcionalQuery(DateTime data, long tipoCalendarioId, string codigoUe)
        {
            Data = data;
            TipoCalendarioId = tipoCalendarioId;
            CodigoUe = codigoUe;
        }

        public DateTime Data { get; }
        public long TipoCalendarioId { get; }
        public string CodigoUe { get; }
    }

    public class ObterDataPossuiEventoLiberacaoExcepcionalQueryValidator : AbstractValidator<ObterDataPossuiEventoLiberacaoExcepcionalQuery>
    {
        public ObterDataPossuiEventoLiberacaoExcepcionalQueryValidator()
        {

            RuleFor(c => c.TipoCalendarioId)
            .NotEmpty()
            .WithMessage("O tipo de calendário deve ser informado.");


            RuleFor(c => c.CodigoUe)
            .NotEmpty()
            .WithMessage("O código da UE deve ser informado.");


            RuleFor(c => c.Data)
            .NotEmpty()
            .WithMessage("A data deve ser informada.");
        }
    }

}
