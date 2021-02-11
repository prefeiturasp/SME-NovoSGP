using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class RemoverResponsavelEncaminhamentoAEECommand : IRequest<bool>
    {
        public RemoverResponsavelEncaminhamentoAEECommand(long encaminhamentoId)
        {
            EncaminhamentoId = encaminhamentoId;
        }

        public long EncaminhamentoId { get; set; }
    }

    public class RemoverResponsavelEncaminhamentoAEECommandValidator : AbstractValidator<RemoverResponsavelEncaminhamentoAEECommand>
    {
        public RemoverResponsavelEncaminhamentoAEECommandValidator()
        {
            RuleFor(x => x.EncaminhamentoId)
                   .GreaterThan(0)
                    .WithMessage("O Id do Encaminhamento deve ser informado!");
        }
    }
}
