using FluentValidation;
using SME.SGP.Dominio;


namespace SME.SGP.Infra.Dtos
{
    public class SalvarNotificacaoCartaIntencoesObservacaoDto
    {
        public SalvarNotificacaoCartaIntencoesObservacaoDto(Turma turma, Usuario usuario, long cartaIntencoesObservacaoId, string observacao)
        {
            Turma = turma;
            Usuario = usuario;
            CartaIntencoesObservacaoId = cartaIntencoesObservacaoId;
            Observacao = observacao;
        }

        public Turma Turma { get; set; }
        public Usuario Usuario { get; set; }

        public long CartaIntencoesObservacaoId { get; set; }
        public string Observacao { get; set; }
    }

    public class NotificarNovaCartaIntencoesObservacaoDtoValidator : AbstractValidator<SalvarNotificacaoCartaIntencoesObservacaoDto>
    {
        public NotificarNovaCartaIntencoesObservacaoDtoValidator()
        {
            RuleFor(c => c.Turma)
                .NotEmpty()
                .WithMessage("A turma deve ser informada.");

            RuleFor(c => c.Usuario)
                .NotEmpty()
                .WithMessage("O usuário deve ser informado.");

            RuleFor(c => c.CartaIntencoesObservacaoId)
                .NotEmpty()
                .WithMessage("A Observação da Carta de Intenções deve ser informado.");

            RuleFor(c => c.Turma.Ue)
                .NotEmpty()
                .WithMessage("A UE deve ser informada.");

            RuleFor(c => c.Turma.Ue.Dre)
                .NotEmpty()
                .WithMessage("A Dre deve ser informada.");
        }
    }
}
