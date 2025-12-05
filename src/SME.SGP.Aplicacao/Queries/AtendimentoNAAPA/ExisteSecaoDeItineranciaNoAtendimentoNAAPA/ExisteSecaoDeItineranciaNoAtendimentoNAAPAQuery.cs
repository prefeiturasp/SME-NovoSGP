using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao.Queries
{
    public class ExisteSecaoDeItineranciaNoAtendimentoNAAPAQuery : IRequest<bool>
    {
        public ExisteSecaoDeItineranciaNoAtendimentoNAAPAQuery(long id)
        {
            this.Id = id;
        }

        public long Id { get; set; }
    }

    public class ExisteSecaoDeItineranciaNoAtendimentoNAAPAQueryValidator : AbstractValidator<ExisteSecaoDeItineranciaNoAtendimentoNAAPAQuery>
    {
        public ExisteSecaoDeItineranciaNoAtendimentoNAAPAQueryValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty()
                .WithMessage("O id do atendimento NAAPA deve ser informado para verificar a existência da seção de itinerância do encaminhamento NAAPA.");
        }
    }
}
