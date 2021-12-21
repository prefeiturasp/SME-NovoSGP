using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterListaFrequenciaPorAulaQuery : IRequest<IEnumerable<RegistroAusenciaAluno>>
    {
        public ObterListaFrequenciaPorAulaQuery(long aulaId)
        {
            AulaId = aulaId;
        }

        public long AulaId { get; }
    }

    public class ObterListaFrequenciaPorAulaQueryValidator : AbstractValidator<ObterListaFrequenciaPorAulaQuery>
    {
        public ObterListaFrequenciaPorAulaQueryValidator()
        {
            RuleFor(a => a.AulaId)
                .NotEmpty()
                .WithMessage("A aula deve ser informada.");
        }
    }
}