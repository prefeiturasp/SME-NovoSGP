using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ExcluirNotificacaoCartaIntencoesObservacaoCommand : IRequest<bool>
    {
        public ExcluirNotificacaoCartaIntencoesObservacaoCommand(long cartaIntencoesObservacaoId)
        {
            CartaIntencoesObservacaoId = cartaIntencoesObservacaoId;
        }

        public long CartaIntencoesObservacaoId { get; set; }
    }


    public class ExcluirNotificacaoCartaIntencoesObservacaoCommandValidator : AbstractValidator<ExcluirNotificacaoCartaIntencoesObservacaoCommand>
    {
        public ExcluirNotificacaoCartaIntencoesObservacaoCommandValidator()
        {
            RuleFor(c => c.CartaIntencoesObservacaoId)
                .NotEmpty()
                .WithMessage("A observação deve ser informada.");
        }
    }
}
