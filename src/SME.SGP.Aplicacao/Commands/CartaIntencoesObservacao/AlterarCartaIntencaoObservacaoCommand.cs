using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class AlterarCartaIntencaoObservacaoCommand : IRequest<AuditoriaDto>
    {
        public AlterarCartaIntencaoObservacaoCommand(string observacao, long observacaoId, long usuarioId)
        {
            Observacao = observacao;
            ObservacaoId = observacaoId;
            UsuarioId = usuarioId;
        }

        public string Observacao { get; set; }
        public long ObservacaoId { get; set; }

        public long UsuarioId { get; set; }
    }


    public class AlterarCartaIntencaoObservacaoCommandValidator : AbstractValidator<AlterarCartaIntencaoObservacaoCommand>
    {
        public AlterarCartaIntencaoObservacaoCommandValidator()
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
