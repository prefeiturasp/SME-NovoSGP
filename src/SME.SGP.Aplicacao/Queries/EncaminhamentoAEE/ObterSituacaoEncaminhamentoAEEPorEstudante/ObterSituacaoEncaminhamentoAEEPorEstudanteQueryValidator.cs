using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterSituacaoEncaminhamentoAEEPorEstudanteQueryValidator : AbstractValidator<ObterSituacaoEncaminhamentoAEEPorEstudanteQuery>
    {
        public ObterSituacaoEncaminhamentoAEEPorEstudanteQueryValidator()
        {
            RuleFor(a => a.EstudanteCodigo)
                .NotEmpty()
                .NotNull()
                .WithMessage("O código do estudante/criança deve ser informado para consulta de seu Encaminhamento AEE");

            RuleFor(a => a.UeCodigo)
                .NotEmpty()
                .NotNull()
                .WithMessage("O código da UE do estudante deve ser informado para consulta do seu Encaminhamento AEE");
        }
    }
}
