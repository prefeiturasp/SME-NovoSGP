using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ExcluirObservacaoDiarioBordoCommand : IRequest<bool>
    {
        public ExcluirObservacaoDiarioBordoCommand(long diarioBordoId, long? usuarioId = null)
        {
            DiarioBordoId = diarioBordoId;
            UsuarioId = usuarioId;
        }

        public long DiarioBordoId { get; set; }
        public long? UsuarioId { get; set; }
    }


    public class ExcluirObservacaoDiarioBordoCommandValidator : AbstractValidator<ExcluirObservacaoDiarioBordoCommand>
    {
        public ExcluirObservacaoDiarioBordoCommandValidator()
        {
            RuleFor(c => c.DiarioBordoId)
                .NotEmpty()
                .WithMessage("O id do diario de bordo deve ser informado.");
        }
    }
}
