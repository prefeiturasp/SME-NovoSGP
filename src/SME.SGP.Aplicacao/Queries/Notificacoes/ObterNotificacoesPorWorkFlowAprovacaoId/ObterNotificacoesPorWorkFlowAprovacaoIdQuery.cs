using System.Collections.Generic;
using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterNotificacoesPorWorkFlowAprovacaoIdQuery : IRequest<IEnumerable<Notificacao>>
    {
        public ObterNotificacoesPorWorkFlowAprovacaoIdQuery(long workFlowAprovacaoId)
        {
            WorkFlowAprovacaoId = workFlowAprovacaoId;
        }
        public long WorkFlowAprovacaoId { get; }
    }

    public class ObterNotificacoesPorWorkFlowAprovacaoIdQueryValidator : AbstractValidator<ObterNotificacoesPorWorkFlowAprovacaoIdQuery>
    {
        public ObterNotificacoesPorWorkFlowAprovacaoIdQueryValidator()
        {
            RuleFor(a => a.WorkFlowAprovacaoId)
                .NotEmpty()
                .WithMessage("O id do workFlowAprovacao deve ser informado para consulta de notificações");
        }
    }
}
