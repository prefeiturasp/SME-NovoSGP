using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterCompensacaoAusenciaDisciplinaRegenciaPorCompensacaoQuery : IRequest<IEnumerable<CompensacaoAusenciaDisciplinaRegencia>>
    {
        public ObterCompensacaoAusenciaDisciplinaRegenciaPorCompensacaoQuery(long compensacaoAusenciaId)
        {
            CompensacaoAusenciaId = compensacaoAusenciaId;
        }

        public long CompensacaoAusenciaId { get; }
    }

    public class ObterCompensacaoAusenciaDisciplinaRegenciaPorCompensacaoQueryValidator : AbstractValidator<ObterCompensacaoAusenciaDisciplinaRegenciaPorCompensacaoQuery>
    {
        public ObterCompensacaoAusenciaDisciplinaRegenciaPorCompensacaoQueryValidator()
        {
            RuleFor(x => x.CompensacaoAusenciaId)
                .GreaterThan(0)
                .WithMessage("O código da compensação ausência deve ser preenchido para obter compensação ausência disciplinas regência.");
        }
    }
}
