using FluentValidation;
using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterIdsNotificacaoPorInformeIdQuery : IRequest<IEnumerable<long>>
    {
        public ObterIdsNotificacaoPorInformeIdQuery(long informaId)
        {
            InformeId = informaId;
        }
        public long InformeId { get; }
    }

    public class ObterIdsNotificacaoPorInformeIdQueryValidator : AbstractValidator<ObterIdsNotificacaoPorInformeIdQuery>
    {
        public ObterIdsNotificacaoPorInformeIdQueryValidator()
        {
            RuleFor(c => c.InformeId)
                .GreaterThan(0)
                .WithMessage("O Id do infomativo deve ser informado para a pesquisa.");
        }
    }
}