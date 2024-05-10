using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class AlterarObservacaoDiarioBordoCommand : IRequest<AuditoriaDto>
    {
        public AlterarObservacaoDiarioBordoCommand(string observacao, long observacaoId, long usuarioId, IEnumerable<long> usuariosIdNotificacao)
        {
            Observacao = observacao;
            ObservacaoId = observacaoId;
            UsuarioId = usuarioId;
            UsuariosIdNotificacao = usuariosIdNotificacao;
        }

        public string Observacao { get; set; }
        public long ObservacaoId { get; set; }
        public long UsuarioId { get; set; }
        public IEnumerable<long> UsuariosIdNotificacao { get; set; }
    }


    public class AlterarObservacaoDiarioBordoCommandValidator : AbstractValidator<AlterarObservacaoDiarioBordoCommand>
    {
        public AlterarObservacaoDiarioBordoCommandValidator()
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
