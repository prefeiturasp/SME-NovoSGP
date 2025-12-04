using FluentValidation;
using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterIdsNotificacaoPorNAAPAIdQuery : IRequest<IEnumerable<long>>
    {
        public ObterIdsNotificacaoPorNAAPAIdQuery(long encaminhamentoNAAPAId)
        {
            EncaminhamentoNAAPAId = encaminhamentoNAAPAId;
        }
        public long EncaminhamentoNAAPAId { get; }
    }

    public class ObterIdsNotificacaoPorNAAPAIdQueryValidator : AbstractValidator<ObterIdsNotificacaoPorNAAPAIdQuery>
    {
        public ObterIdsNotificacaoPorNAAPAIdQueryValidator()
        {
            RuleFor(c => c.EncaminhamentoNAAPAId)
                .GreaterThan(0)
                .WithMessage("O Id do Encaminhamento NAAPA deve ser informado para a pesquisa.");
        }
    }
}