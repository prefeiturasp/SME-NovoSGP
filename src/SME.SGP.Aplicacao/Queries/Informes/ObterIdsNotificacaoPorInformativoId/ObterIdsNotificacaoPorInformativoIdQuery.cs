using FluentValidation;
using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterIdsNotificacaoPorInformativoIdQuery : IRequest<IEnumerable<long>>
    {
        public ObterIdsNotificacaoPorInformativoIdQuery(long informativoId)
        {
            InformativoId = informativoId;
        }
        public long InformativoId { get; }
    }

    public class ObterIdsNotificacaoPorInformeIdQueryValidator : AbstractValidator<ObterIdsNotificacaoPorInformativoIdQuery>
    {
        public ObterIdsNotificacaoPorInformeIdQueryValidator()
        {
            RuleFor(c => c.InformativoId)
                .GreaterThan(0)
                .WithMessage("O Id do informativo deve ser informado para a pesquisa.");
        }
    }
}