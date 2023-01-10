using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao.Queries
{
    public class ExisteSecaoDeItineranciaNoEncaminhamentoNAAPAQuery : IRequest<bool>
    {
        public ExisteSecaoDeItineranciaNoEncaminhamentoNAAPAQuery(long id)
        {
            this.Id = id;
        }

        public long Id { get; set; }
    }

    public class ExisteSecaoDeItineranciaNoEncaminhamentoNAAPAQueryValidator : AbstractValidator<ExisteSecaoDeItineranciaNoEncaminhamentoNAAPAQuery>
    {
        public ExisteSecaoDeItineranciaNoEncaminhamentoNAAPAQueryValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty()
                .WithMessage("O id do encaminhamento NAAPA deve ser informada para verificar a existência da seção de itinerância do encaminhamento NAAPA.");
        }
    }
}
