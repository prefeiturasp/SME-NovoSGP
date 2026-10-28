using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirNotificacaoDevolutivaCommand : IRequest<bool>
    {
        public ExcluirNotificacaoDevolutivaCommand(long devolutivaId)
        {
            DevolutivaId = devolutivaId;
        }

        public long DevolutivaId { get; set; }
    }


    public class ExcluirNotificacaoDevolutivaCommandValidator : AbstractValidator<ExcluirNotificacaoDevolutivaCommand>
    {
        public ExcluirNotificacaoDevolutivaCommandValidator()
        {
            RuleFor(c => c.DevolutivaId)
                .NotEmpty()
                .WithMessage("A devolutiva deve ser informada.");
        }
    }
}
