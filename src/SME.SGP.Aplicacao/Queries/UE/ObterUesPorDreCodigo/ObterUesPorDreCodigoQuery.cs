using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterUesPorDreCodigoQuery : IRequest<IEnumerable<Ue>>
    {
        public ObterUesPorDreCodigoQuery(long dreId)
        {
            DreId = dreId;
        }

        public long DreId { get; set; }
    }

    public class ObterUesPorDreCodigoQueryValidator : AbstractValidator<ObterUesPorDreCodigoQuery>
    {
        public ObterUesPorDreCodigoQueryValidator()
        {
            RuleFor(c => c.DreId)
            .NotEmpty()
            .WithMessage("O código da dre deve ser informado para consulta das UEs.");
        }
    }
}
