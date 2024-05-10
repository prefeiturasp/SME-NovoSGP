using FluentValidation;

namespace SME.SGP.Aplicacao.Queries.Evento.ObterDataPossuiEventoLiberacaoExcepcional
{
    public class ObterSecoesPorEtapaDeEncaminhamentoQueryValidator : AbstractValidator<ObterSecoesPorEtapaDeEncaminhamentoQuery>
    {
        public ObterSecoesPorEtapaDeEncaminhamentoQueryValidator()
        {
            RuleFor(c => c.Etapas)
            .NotEmpty()
            .WithMessage("As Etapas devem ser informadas.");
        }
    }

}
