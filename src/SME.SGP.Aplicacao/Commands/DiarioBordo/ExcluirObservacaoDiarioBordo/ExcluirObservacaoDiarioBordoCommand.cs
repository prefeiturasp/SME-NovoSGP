using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ExcluirObservacaoDiarioBordoCommand : IRequest<bool>
    {
        public ExcluirObservacaoDiarioBordoCommand(long observacaoId, long? usuarioId = null)
        {
            ObservacaoId = observacaoId;
            UsuarioId = usuarioId;
        }

        public long ObservacaoId { get; set; }
        public long? UsuarioId { get; set; }
    }


    public class ExcluirObservacaoDiarioBordoCommandValidator : AbstractValidator<ExcluirObservacaoDiarioBordoCommand>
    {
        public ExcluirObservacaoDiarioBordoCommandValidator()
        {
            RuleFor(c => c.ObservacaoId)
                .NotEmpty()
                .WithMessage("A observação deve ser informada.");
        }
    }
}
