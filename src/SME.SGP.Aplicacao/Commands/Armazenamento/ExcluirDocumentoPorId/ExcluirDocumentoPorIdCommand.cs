using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirDocumentoPorIdCommand : IRequest<bool>
    {
        public ExcluirDocumentoPorIdCommand(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }

    public class ExcluirDocumentoPorIdCommandValidator : AbstractValidator<ExcluirDocumentoPorIdCommand>
    {
        public ExcluirDocumentoPorIdCommandValidator()
        {
            RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("O identificador do documento deve ser informado para exclusão.");
        }
    }
}
