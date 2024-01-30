using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirInformesCommand : IRequest<bool>
    {
        public ExcluirInformesCommand(long id)
        {
            Id = id;
        }
        public long Id { get; }
    }

    public class ExcluirInformesCommandValidator : AbstractValidator<ExcluirInformesCommand>
    {
        public ExcluirInformesCommandValidator()
        {
            RuleFor(c => c.Id)
                .GreaterThan(0)
                .WithMessage("O Id do infomes deve ser informado para exclusão.");
        }
    }
}
