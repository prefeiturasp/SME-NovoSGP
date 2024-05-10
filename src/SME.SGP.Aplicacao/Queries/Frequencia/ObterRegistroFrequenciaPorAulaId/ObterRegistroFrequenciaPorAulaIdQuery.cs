using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterRegistroFrequenciaPorAulaIdQuery : IRequest<RegistroFrequencia>
    {
        public ObterRegistroFrequenciaPorAulaIdQuery(long aulaId)
        {
            AulaId = aulaId;
        }

        public long AulaId { get; set; }
    }

    public class ObterRegistroFrequenciaPorAulaIdQueryValidator : AbstractValidator<ObterRegistroFrequenciaPorAulaIdQuery>
    {
        public ObterRegistroFrequenciaPorAulaIdQueryValidator()
        {
            RuleFor(x => x.AulaId)
             .NotEmpty()
             .WithMessage("O id da aula deve ser informado.");
        }
    }
}
