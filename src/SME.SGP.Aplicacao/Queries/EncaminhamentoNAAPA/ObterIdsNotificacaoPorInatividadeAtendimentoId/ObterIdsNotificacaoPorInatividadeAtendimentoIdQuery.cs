using FluentValidation;
using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterIdsNotificacaoPorInatividadeAtendimentoIdQuery : IRequest<IEnumerable<long>>
    {
        public ObterIdsNotificacaoPorInatividadeAtendimentoIdQuery(long encaminhamentoNAAPAId)
        {
            EncaminhamentoNAAPAId = encaminhamentoNAAPAId;
        }
        public long EncaminhamentoNAAPAId { get; }
    }

    public class ObterIdsNotificacaoPorInatividadeAtendimentoIdQueryValidator : AbstractValidator<ObterIdsNotificacaoPorInatividadeAtendimentoIdQuery>
    {
        public ObterIdsNotificacaoPorInatividadeAtendimentoIdQueryValidator()
        {
            RuleFor(c => c.EncaminhamentoNAAPAId)
                .GreaterThan(0)
                .WithMessage("O Id do Encaminhamento NAAPA deve ser informado para a pesquisa.");
        }
    }
}