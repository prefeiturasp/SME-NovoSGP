using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterRespostaPorSecaoPAPQuery : IRequest<IEnumerable<RelatorioPeriodicoPAPResposta>>
    {
        public ObterRespostaPorSecaoPAPQuery(long secaoId)
        {
            SecaoId = secaoId;
        }

        public long SecaoId {  get; set; }
    }

    public class ObterRespostaPorSecaoPAPQueryValidator : AbstractValidator<ObterRespostaPorSecaoPAPQuery>
    {
        public ObterRespostaPorSecaoPAPQueryValidator()
        {
            RuleFor(x => x.SecaoId)
                .NotEmpty()
                .WithMessage("O id da seção pap deve ser informado para resposta.");
        }
    }
}
