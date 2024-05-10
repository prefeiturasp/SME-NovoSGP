using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeDeAnexosNaItineranciaQuery : IRequest<int>
    {
        public ObterQuantidadeDeAnexosNaItineranciaQuery(long itineranciaId)
        {
            ItineranciaId = itineranciaId;
        }

        public long ItineranciaId { get; set; }
    }

    public class ObterQuantidadeDeAnexosNaItineranciaQueryValidator : AbstractValidator<ObterQuantidadeDeAnexosNaItineranciaQuery>
    {
        public ObterQuantidadeDeAnexosNaItineranciaQueryValidator()
        {
            RuleFor(x => x.ItineranciaId).GreaterThan(0).WithMessage("Informe a Itinerancia para consultar se existe anexos");
        }
    }
}