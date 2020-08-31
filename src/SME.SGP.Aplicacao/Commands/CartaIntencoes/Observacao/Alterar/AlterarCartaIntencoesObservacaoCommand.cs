using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class AlterarCartaIntencoesObservacaoCommand : IRequest<AuditoriaDto>
    {
        public AlterarCartaIntencoesObservacaoCommand(string observacao, long observacaoId, long usuarioId)
        {
            Observacao = observacao;
            ObservacaoId = observacaoId;
            UsuarioId = usuarioId;
        }

        public string Observacao { get; set; }
        public long ObservacaoId { get; set; }

        public long UsuarioId { get; set; }
    }


    public class AlterarCartaIntencoesObservacaoCommandValidator : AbstractValidator<AlterarCartaIntencoesObservacaoCommand>
    {
        public AlterarCartaIntencoesObservacaoCommandValidator()
        {
            RuleFor(c => c.ObservacaoId)
                .NotEmpty()
                .WithMessage("O id da observação deve ser informado.");

            RuleFor(c => c.UsuarioId)
               .NotEmpty()
               .WithMessage("O id do usuário deve ser informado.");

            RuleFor(c => c.Observacao)
                .NotEmpty()
                .WithMessage("A observação deve ser informada.");
        }
    }
}
