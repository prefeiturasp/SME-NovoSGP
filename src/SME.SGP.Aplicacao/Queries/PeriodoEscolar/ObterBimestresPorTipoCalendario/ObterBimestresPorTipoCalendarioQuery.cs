using FluentValidation;
using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterBimestresPorTipoCalendarioQuery : IRequest<IEnumerable<int>>
    {
        public ObterBimestresPorTipoCalendarioQuery(long tipoCalendarioId)
        {
            TipoCalendarioId = tipoCalendarioId;
        }

        public long TipoCalendarioId { get; }
    }

    public class ObterBimestresPorTipoCalendarioQueryValidator : AbstractValidator<ObterBimestresPorTipoCalendarioQuery>
    {
        public ObterBimestresPorTipoCalendarioQueryValidator()
        {
            RuleFor(a => a.TipoCalendarioId)
                .NotEmpty()
                .WithMessage("O identificador do tipo de calendário deve ser informado para consulta dos bimestres");
        }
    }
}
