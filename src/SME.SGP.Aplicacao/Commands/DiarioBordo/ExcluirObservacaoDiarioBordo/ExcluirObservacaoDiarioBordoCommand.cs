using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirObservacaoDiarioBordoCommand : IRequest<bool>
    {
        public ExcluirObservacaoDiarioBordoCommand(long diarioBordoObservacaoId)
        {
            DiarioBordoObservacaoId = diarioBordoObservacaoId;
        }

        public long DiarioBordoObservacaoId { get; set; }
    }


    public class ExcluirObservacaoDiarioBordoCommandValidator : AbstractValidator<ExcluirObservacaoDiarioBordoCommand>
    {
        public ExcluirObservacaoDiarioBordoCommandValidator()
        {
            RuleFor(c => c.DiarioBordoObservacaoId)
                .NotEmpty()
                .WithMessage("O id da observacao do diario de bordo deve ser informado.");
        }
    }
}
