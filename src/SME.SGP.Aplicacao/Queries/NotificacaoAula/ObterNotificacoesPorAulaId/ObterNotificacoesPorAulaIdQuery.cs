using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterNotificacoesPorAulaIdQuery: IRequest<IEnumerable<NotificacaoAula>>
    {
        public ObterNotificacoesPorAulaIdQuery(long aulaId)
        {
            AulaId = aulaId;
        }

        public long AulaId { get; set; }
    }

    public class ObterNotificacoesPorAulaIdQueryValidator: AbstractValidator<ObterNotificacoesPorAulaIdQuery>
    {
        public ObterNotificacoesPorAulaIdQueryValidator()
        {
            RuleFor(a => a.AulaId)
                .NotEmpty()
                .WithMessage("O Id da aula é necessário para obter suas notificações.");
        }
    }
}
