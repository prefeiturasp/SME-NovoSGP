using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class PlanejamentoAnualPeriodoPossuiObjetivosQuery : IRequest<bool>
    {
        public PlanejamentoAnualPeriodoPossuiObjetivosQuery(long planejamentoAnualPeriodoId)
        {
            PlanejamentoAnualPeriodoId = planejamentoAnualPeriodoId;
        }

        public long PlanejamentoAnualPeriodoId { get; set; }
    }

    public class PlanejamentoAnualPossuiObjetivosQueryValidator : AbstractValidator<PlanejamentoAnualPeriodoPossuiObjetivosQuery>
    {
        public PlanejamentoAnualPossuiObjetivosQueryValidator()
        {
            RuleFor(c => c.PlanejamentoAnualPeriodoId)
            .NotEmpty()
            .WithMessage("O id do planejamento deve ser informado para consulta da existencia de objetivos.");

        }
    }
}
