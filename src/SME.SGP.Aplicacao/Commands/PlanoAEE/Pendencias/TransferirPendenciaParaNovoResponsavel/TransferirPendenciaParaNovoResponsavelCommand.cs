using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class TransferirPendenciaParaNovoResponsavelCommand : IRequest<bool>
    {
        public TransferirPendenciaParaNovoResponsavelCommand(long planoAeeId, long responsavelId)
        {
            PlanoAeeId = planoAeeId;
            ResponsavelId = responsavelId;
        }

        public long PlanoAeeId { get; }
        public long ResponsavelId { get; }
    }

    public class TransferirPendenciaParaNovoResponsavelCommandValidator : AbstractValidator<TransferirPendenciaParaNovoResponsavelCommand>
    {
        public TransferirPendenciaParaNovoResponsavelCommandValidator()
        {
            RuleFor(transferencia => transferencia.PlanoAeeId)
                .NotEmpty()
                .WithMessage("O id do plano deve ser informado!");

            RuleFor(transferencia => transferencia.ResponsavelId)
                .NotEmpty()
                .WithMessage("O id do responsável deve ser informado!");
        }
    }
}
