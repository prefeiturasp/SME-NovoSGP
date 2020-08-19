using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ExcluirObservacaoDiarioBordoCommand : IRequest<bool>
    {
        public ExcluirObservacaoDiarioBordoCommand(long observacaoId, long usuarioId)
        {
            ObservacaoId = observacaoId;
            UsuarioId = usuarioId;
        }

        public long ObservacaoId { get; set; }
        public long UsuarioId { get; set; }
    }


    public class ExcluirObservacaoDiarioBordoCommandValidator : AbstractValidator<ExcluirObservacaoDiarioBordoCommand>
    {
        public ExcluirObservacaoDiarioBordoCommandValidator()
        {
            RuleFor(c => c.UsuarioId)
                .NotEmpty()
                .WithMessage("O usuário deve ser informado.");

            RuleFor(c => c.ObservacaoId)
                .NotEmpty()
                .WithMessage("A observação deve ser informada.");
        }
    }
}
