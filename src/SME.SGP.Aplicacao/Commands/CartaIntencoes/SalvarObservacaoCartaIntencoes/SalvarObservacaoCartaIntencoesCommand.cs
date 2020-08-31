using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class SalvarObservacaoCartaIntencoesCommand : IRequest<AuditoriaDto>
    {
        public SalvarObservacaoCartaIntencoesCommand(string observacao, long cartaIntencoesId, long usuarioId)
        {
            Observacao = observacao;
            CartaIntencoesId = cartaIntencoesId;
            UsuarioId = usuarioId;
        }

        public string Observacao { get; set; }
        public long CartaIntencoesId { get; set; }
        public long UsuarioId { get; set; }
    }

    public class SalvarObservacaoCartaIntencoesCommandValidator : AbstractValidator<SalvarObservacaoCartaIntencoesCommand>
    {
        public SalvarObservacaoCartaIntencoesCommandValidator()
        {
            RuleFor(c => c.Observacao)
               .NotEmpty()
               .WithMessage("O campo descrição deve ser informado.");

            RuleFor(c => c.UsuarioId)
              .NotEmpty()
              .WithMessage("O campo id do usuário logado deve ser informado.");
        }
    }
}
