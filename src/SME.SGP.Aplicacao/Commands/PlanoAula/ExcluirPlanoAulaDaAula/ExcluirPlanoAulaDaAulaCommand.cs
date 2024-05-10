using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPlanoAulaDaAulaCommand: IRequest<bool>
    {
        public ExcluirPlanoAulaDaAulaCommand(long aulaId)
        {
            AulaId = aulaId;
        }

        public long AulaId { get; set; }
    }

    public class ExcluirPlanoAulaDaAulaCommandValidator: AbstractValidator<ExcluirPlanoAulaDaAulaCommand>
    {
        public ExcluirPlanoAulaDaAulaCommandValidator()
        {
            RuleFor(a => a.AulaId)
                .NotEmpty()
                .WithMessage("O Id da aula deve ser informado para exclusão de seu plano de aula.");
        }
    }
}
