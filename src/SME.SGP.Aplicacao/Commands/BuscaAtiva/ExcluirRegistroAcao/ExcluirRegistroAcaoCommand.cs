using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirRegistroAcaoCommand : IRequest<bool>
    {
        public ExcluirRegistroAcaoCommand(long registroAcaoId)
        {
            RegistroAcaoId = registroAcaoId;
        }

        public long RegistroAcaoId { get; }
    }

    public class ExcluirRegistroAcaoCommandValidator : AbstractValidator<ExcluirRegistroAcaoCommand>
    {
        public ExcluirRegistroAcaoCommandValidator()
        {

            RuleFor(c => c.RegistroAcaoId)
            .NotEmpty()
            .WithMessage("O id do registro de ação deve ser informado para exclusão.");

        }
    }
}
