using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterCompensacaoAusenciaAlunoPorCompensacaoQuery : IRequest<IEnumerable<CompensacaoAusenciaAluno>>
    {
        public long CompensacaoId { get; set; }

        public ObterCompensacaoAusenciaAlunoPorCompensacaoQuery(long compensacaoId)
        {
            CompensacaoId = compensacaoId;
        }        
    }

    public class ObterCompensacaoAusenciaAlunoPorCompensacaoQueryValidator : AbstractValidator<ObterCompensacaoAusenciaAlunoPorCompensacaoQuery>
    {
        public ObterCompensacaoAusenciaAlunoPorCompensacaoQueryValidator()
        {
            RuleFor(x => x.CompensacaoId)
                .NotEmpty()
                .WithMessage("A compensacao deve ser informada.");
        }
    }
}