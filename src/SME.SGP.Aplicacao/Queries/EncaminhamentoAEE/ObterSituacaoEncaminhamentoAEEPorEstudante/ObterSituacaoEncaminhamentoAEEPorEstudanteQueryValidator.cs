using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterSituacaoEncaminhamentoAEEPorEstudanteQueryValidator : AbstractValidator<ObterSituacaoEncaminhamentoAEEPorEstudanteQuery>
    {
        public ObterSituacaoEncaminhamentoAEEPorEstudanteQueryValidator()
        {
            RuleFor(a => a.CodigoEstudante)
                .NotEmpty()
                .WithMessage("O código do estudante/criança deve ser informado para consulta de seu Encaminhamento AEE");
        }
    }
}
