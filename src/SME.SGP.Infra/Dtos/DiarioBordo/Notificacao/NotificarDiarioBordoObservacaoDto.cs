using FluentValidation;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra.Dtos
{
    public class NotificarDiarioBordoObservacaoDto
    {
        public NotificarDiarioBordoObservacaoDto(long diarioBordoId, string observacao, Usuario usuario, long observacaoId, IEnumerable<string> usuariosRfNotificacao = null)
        {
            DiarioBordoId = diarioBordoId;
            Observacao = observacao;
            Usuario = usuario;
            ObservacaoId = observacaoId;
            UsuariosNotificacao = usuariosRfNotificacao;
        }
        public long ObservacaoId { get; set; }
        public string Observacao { get; set; }
        public long DiarioBordoId { get; set; }
        public Usuario Usuario { get; set; }
        public IEnumerable<string> UsuariosNotificacao { get; set; }
    }

    public class NotificarDiarioBordoObservacaoDtoValidator : AbstractValidator<NotificarDiarioBordoObservacaoDto>
    {
        public NotificarDiarioBordoObservacaoDtoValidator()
        {
            RuleFor(c => c.DiarioBordoId)
                .NotEmpty()
                .WithMessage("O Id do diario de bordo deve ser informado");

            RuleFor(c => c.Usuario)
                .NotEmpty()
                .WithMessage("O usuario deve ser informado");           
        }
    }    
}
