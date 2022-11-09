using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class SalvarPendenciaAoExcluirDiarioBordoCommand : IRequest
    {
        public SalvarPendenciaAoExcluirDiarioBordoCommand(long diarioBordoId)
        {
            DiarioBordoId = diarioBordoId;
        }
        public long DiarioBordoId { get; set; }
    }

    public class SalvarPendenciaAoExcluirDiarioBordoCommandValidator : AbstractValidator<SalvarPendenciaAoExcluirDiarioBordoCommand>
    {
        public SalvarPendenciaAoExcluirDiarioBordoCommandValidator()
        {
            RuleFor(c => c.DiarioBordoId)
            .NotEmpty()
            .WithMessage("O Id do Diário de Bordo excluído deve ser preenchido para geração da nova Pendência de Diário de Bordo.");
        }
    }
}
