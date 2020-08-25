using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class AulaExisteQuery: IRequest<bool>
    {
        public AulaExisteQuery(long aulaId)
        {
            AulaId = aulaId;
        }

        public long AulaId { get; set; }
    }

    public class AulaExisteQueryValidator : AbstractValidator<AulaExisteQuery>
    {
        public AulaExisteQueryValidator()
        {
            RuleFor(a => a.AulaId)
                .Must(a => a > 0)
                .WithMessage("O Id da aula deve ser informado para consulta");
        }
    }
}
