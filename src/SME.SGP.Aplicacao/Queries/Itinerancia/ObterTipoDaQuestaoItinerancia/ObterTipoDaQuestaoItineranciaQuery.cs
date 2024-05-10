using System.Collections.Generic;
using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterTipoDaQuestaoItineranciaQuery : IRequest<List<QuestaoTipoDto>>
    {
        public ObterTipoDaQuestaoItineranciaQuery(long itineranciaId)
        {
            ItineranciaId = itineranciaId;
        }

        public long ItineranciaId { get; set; }
    }

    public class ObterTipoDaQuestaoItineranciaQueryValidator : AbstractValidator<ObterTipoDaQuestaoItineranciaQuery>
    {
        public ObterTipoDaQuestaoItineranciaQueryValidator()
        {
            RuleFor(x => x.ItineranciaId).GreaterThanOrEqualTo(0).WithMessage("A Itinerancia deve ser informada para obter o tipo da questão");
        }
    }
}