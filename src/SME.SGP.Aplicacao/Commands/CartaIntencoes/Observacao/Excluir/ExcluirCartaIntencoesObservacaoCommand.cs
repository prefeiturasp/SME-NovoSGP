using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ExcluirCartaIntencoesObservacaoCommand : IRequest<bool>
    {
        public ExcluirCartaIntencoesObservacaoCommand(long observacaoId, long usuarioId)
        {
            ObservacaoId = observacaoId;
            UsuarioId = usuarioId;
        }

        public long ObservacaoId { get; set; }
        public long UsuarioId { get; set; }
    }


    public class ExcluirCartaIntencoesObservacaoCommandValidator : AbstractValidator<ExcluirCartaIntencoesObservacaoCommand>
    {
        public ExcluirCartaIntencoesObservacaoCommandValidator()
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
