using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class RemoverConsolidacoesDiariosBordoCommand : IRequest
    {
        public RemoverConsolidacoesDiariosBordoCommand(int anoLetivo)
        {
            AnoLetivo = anoLetivo;
        }

        public int AnoLetivo { get; }
    }

    public class RemoverConsolidacoesDiariosBordoCommandValidator : AbstractValidator<RemoverConsolidacoesDiariosBordoCommand>
    {
        public RemoverConsolidacoesDiariosBordoCommandValidator()
        {
            RuleFor(a => a.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado para exclusão das consolidações de diários de bordo");
        }
    }
}
