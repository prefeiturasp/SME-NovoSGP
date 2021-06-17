using FluentValidation;
using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterUesCodigosPorDreQuery : IRequest<IEnumerable<string>>
    {
        public ObterUesCodigosPorDreQuery(long dreId)
        {
            DreId = dreId;
        }

        public long DreId { get; set; }
    }

    public class ObterUesIdPorDreQueryValidator : AbstractValidator<ObterUesCodigosPorDreQuery>
    {
        public ObterUesIdPorDreQueryValidator()
        {
            RuleFor(c => c.DreId)
            .NotEmpty()
            .WithMessage("O id dre deve ser informado para consulta dos ids das UEs.");
        }
    }
}
