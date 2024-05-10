using FluentValidation;
using MediatR;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterDataEhDiaLetivoPorTipoCalendarioQuery : IRequest<bool>
    {
        public ObterDataEhDiaLetivoPorTipoCalendarioQuery(long tipoCalendarioId, DateTime data, string codigoDre, string codigoUe)
        {
            TipoCalendarioId = tipoCalendarioId;
            Data = data;
            CodigoDre = codigoDre;
            CodigoUe = codigoUe;
        }

        public long TipoCalendarioId { get; }
        public DateTime Data { get; }
        public string CodigoDre { get; private set; }
        public string CodigoUe { get; private set; }
    }


    public class ObterDataEhDiaLetivoPorTipoCalendarioQueryValidator : AbstractValidator<ObterDataEhDiaLetivoPorTipoCalendarioQuery>
    {
        public ObterDataEhDiaLetivoPorTipoCalendarioQueryValidator()
        {

            RuleFor(c => c.TipoCalendarioId)
            .NotEmpty()
            .WithMessage("O tipo de calendário deve ser informado.");


            RuleFor(c => c.Data)
            .NotEmpty()
            .WithMessage("A data deve ser informada.");


            RuleFor(c => c.CodigoDre)
            .NotEmpty()
            .WithMessage("O código da DRE deve ser informado.");


            RuleFor(c => c.CodigoUe)
            .NotEmpty()
            .WithMessage("O código da UE deve ser informado.");
        }
    }

}
