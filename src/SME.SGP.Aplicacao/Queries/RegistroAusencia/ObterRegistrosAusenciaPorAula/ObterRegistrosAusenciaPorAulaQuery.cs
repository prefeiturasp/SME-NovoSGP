using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterRegistrosAusenciaPorAulaQuery : IRequest<IEnumerable<RegistroFrequenciaAluno>>
    {
        public long AulaId { get; set; }

        public ObterRegistrosAusenciaPorAulaQuery(long aulaId)
        {
            AulaId = aulaId;
        }
    }

    public class ObterRegistrosAusenciaPorAulaQueryValidator : AbstractValidator<ObterRegistrosAusenciaPorAulaQuery>
    {
        public ObterRegistrosAusenciaPorAulaQueryValidator()
        {
            RuleFor(x => x.AulaId)
                .NotEmpty()
                .WithMessage("A aula id deve ser informada.");
        }
    }
}