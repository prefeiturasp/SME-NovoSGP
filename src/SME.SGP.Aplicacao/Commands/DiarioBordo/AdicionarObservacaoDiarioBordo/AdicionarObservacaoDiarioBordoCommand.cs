using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class AdicionarObservacaoDiarioBordoCommand : IRequest<AuditoriaDto>
    {
        public AdicionarObservacaoDiarioBordoCommand(long diarioBordoId, string observacao, long usuarioId, IEnumerable<long> usuariosIdNotificacao)
        {
            DiarioBordoId = diarioBordoId;
            Observacao = observacao;
            UsuarioId = usuarioId;
            UsuariosIdNotificacao = usuariosIdNotificacao;
        }

        public long DiarioBordoId { get; set; }
        public string Observacao { get; set; }
        public long UsuarioId { get; set; }
        public IEnumerable<long> UsuariosIdNotificacao { get; set; }
    }


    public class AdicionarObservacaoCommandValidator : AbstractValidator<AdicionarObservacaoDiarioBordoCommand>
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
