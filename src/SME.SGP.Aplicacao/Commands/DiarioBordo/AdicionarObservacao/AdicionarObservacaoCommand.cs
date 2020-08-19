using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class AdicionarObservacaoCommand : IRequest<AuditoriaDto>
    {
        public AdicionarObservacaoCommand(long diarioBordoId, string observacao, long usuarioId)
        {
            DiarioBordoId = diarioBordoId;
            Observacao = observacao;
            UsuarioId = usuarioId;
        }

        public long DiarioBordoId { get; set; }
        public string Observacao { get; set; }
        public long UsuarioId { get; set; }
    }


    public class AdicionarObservacaoCommandValidator : AbstractValidator<AdicionarObservacaoCommand>
    {
        public AdicionarObservacaoCommandValidator()
        {

            RuleFor(c => c.DiarioBordoId)
                .NotEmpty()
                .WithMessage("O Diário de Bordo deve ser informado.");

            RuleFor(c => c.UsuarioId)
                .NotEmpty()
                .WithMessage("O usuário deve ser informado.");

            RuleFor(c => c.Observacao)
                .NotEmpty()
                .WithMessage("A observação deve ser informada.");
        }
    }
}
