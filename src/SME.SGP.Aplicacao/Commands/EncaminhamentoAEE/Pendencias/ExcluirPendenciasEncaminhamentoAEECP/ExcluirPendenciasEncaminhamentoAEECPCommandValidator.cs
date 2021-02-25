using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPendenciasEncaminhamentoAEECPCommandValidator : AbstractValidator<ExcluirPendenciasEncaminhamentoAEECPCommand>
    {
        public ExcluirPendenciasEncaminhamentoAEECPCommandValidator()
        {
            RuleFor(c => c.EncaminhamentoId)
                .NotEmpty()
                .WithMessage("O id do encaminhamento deve ser informado.");

            RuleFor(c => c.TurmaId)
                .NotEmpty()
                .WithMessage("O id da turma deve ser informado.");
        }
    }
}
