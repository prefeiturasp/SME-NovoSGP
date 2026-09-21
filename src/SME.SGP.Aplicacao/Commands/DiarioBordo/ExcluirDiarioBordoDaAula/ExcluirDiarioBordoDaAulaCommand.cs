using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirDiarioBordoDaAulaCommand: IRequest<bool>
    {
        public ExcluirDiarioBordoDaAulaCommand(long aulaId)
        {
            AulaId = aulaId;
        }

        public long AulaId { get; set; }
    }

    public class ExcluirDiarioBordoDaAulaCommandValidator: AbstractValidator<ExcluirDiarioBordoDaAulaCommand>
    {
        public ExcluirDiarioBordoDaAulaCommandValidator()
        {
            RuleFor(a => a.AulaId)
                .NotEmpty()
                .WithMessage("Necessário informar o Id da aula para exclusão do diário de bordo");
        }
    }
}
