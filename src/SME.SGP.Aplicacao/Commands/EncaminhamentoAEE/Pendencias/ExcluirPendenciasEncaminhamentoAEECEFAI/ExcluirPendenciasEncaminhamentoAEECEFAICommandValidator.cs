using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPendenciasEncaminhamentoAEECEFAICommandValidator : AbstractValidator<ExcluirPendenciasEncaminhamentoAEECEFAICommand>
    {
        public ExcluirPendenciasEncaminhamentoAEECEFAICommandValidator()
        {
            RuleFor(c => c.EncaminhamentoAEEId)
                .NotEmpty()
                .WithMessage("O id do EncaminhamentoAEE deve ser informado.");

            RuleFor(c => c.TurmaId)
                .NotEmpty()
                .WithMessage("O id da turma deve ser informado.");
        }
    }
}
