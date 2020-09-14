using FluentValidation;

namespace SME.SGP.Infra.Dtos
{
    public class ExcluirNotificacaoCartaIntencoesObservacaoDto
    {
        public ExcluirNotificacaoCartaIntencoesObservacaoDto(long cartaIntencoesObservacaoId)
        {
            CartaIntencoesObservacaoId = cartaIntencoesObservacaoId;
        }

        public long CartaIntencoesObservacaoId { get; set; }
        
    }

    public class NotificarExcluirCartaIntencoesObservacaoDtoValidator : AbstractValidator<ExcluirNotificacaoCartaIntencoesObservacaoDto>
    {
        public NotificarExcluirCartaIntencoesObservacaoDtoValidator()
        {
            RuleFor(c => c.CartaIntencoesObservacaoId)
                .NotEmpty()
                .WithMessage("A observação da carta de intenções deve ser informada.");
        }
    }
}
