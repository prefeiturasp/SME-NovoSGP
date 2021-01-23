using FluentValidation;

namespace SME.SGP.Aplicacao.Queries.Evento.ObterDataPossuiEventoLiberacaoExcepcional
{
    public class ObterSecoesPorEtapaDeEncaminhamentoQueryValidator : AbstractValidator<ObterSecoesPorEtapaDeEncaminhamentoQuery>
    {
        public ObterSecoesPorEtapaDeEncaminhamentoQueryValidator()
        {
            RuleFor(c => c.Etapa)
            .NotEmpty()
            .WithMessage("A Etapa deve ser informada.");
        }
    }

}
