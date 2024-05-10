using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class AlterarAvisoDoMuralCommand : IRequest
    {
        public AlterarAvisoDoMuralCommand(long id, string mensagem)
        {
            Id = id;
            Mensagem = mensagem;
        }

        public long Id { get; }
        public string Mensagem { get; }
    }

    public class AlterarAvisoDoMuralCommandValidator : AbstractValidator<AlterarAvisoDoMuralCommand>
    {
        public AlterarAvisoDoMuralCommandValidator()
        {
            RuleFor(a => a.Id)
                .NotEmpty()
                .WithMessage("O id do aviso deve ser informado para alteração");

            RuleFor(a => a.Mensagem)
                .NotEmpty()
                .WithMessage("A mensagem do aviso deve ser informada para alteração");
        }
    }
}
