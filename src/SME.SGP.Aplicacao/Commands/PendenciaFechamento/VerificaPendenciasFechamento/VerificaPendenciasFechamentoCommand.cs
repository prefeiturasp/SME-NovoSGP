using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class VerificaPendenciasFechamentoCommand : IRequest
    {
        public VerificaPendenciasFechamentoCommand(long fechamentoId)
        {
            FechamentoId = fechamentoId;
        }

        public long FechamentoId { get; set; }
    }

    public class VerificaPendenciasFechamentoCommandValidator : AbstractValidator<VerificaPendenciasFechamentoCommand>
    {
        public VerificaPendenciasFechamentoCommandValidator()
        {
            RuleFor(a => a.FechamentoId)
                .NotEmpty()
                .WithMessage("Necessário informar o indentificador do Fechamento");
        }
    }
}
