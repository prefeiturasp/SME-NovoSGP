using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirDiarioBordoCommand : IRequest<bool>
    {
        public ExcluirDiarioBordoCommand(long diarioBordoId)
        {
            DiarioBordoId = diarioBordoId;
        }

        public long DiarioBordoId { get; set; }
    }


    public class ExcluirDiarioBordoCommandValidator : AbstractValidator<ExcluirDiarioBordoCommand>
    {
        public ExcluirDiarioBordoCommandValidator()
        {
            RuleFor(c => c.DiarioBordoId)
                .NotEmpty()
                .WithMessage("O id do diario de bordo deve ser informado para exclusão.");
        }
    }
}
