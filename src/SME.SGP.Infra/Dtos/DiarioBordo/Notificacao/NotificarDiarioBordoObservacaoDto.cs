using FluentValidation;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra.Dtos
{
    public class NotificarDiarioBordoObservacaoDto
    {
        public NotificarDiarioBordoObservacaoDto(long diarioBordoId, long usuarioId)
        {
            DiarioBordoId = diarioBordoId;
            UsuarioId = usuarioId;
        }

        public long DiarioBordoId { get; set; }
        public long UsuarioId { get; set; }
    }

    public class NotificarDiarioBordoObservacaoDtoValidator : AbstractValidator<NotificarDiarioBordoObservacaoDto>
    {
        public NotificarDiarioBordoObservacaoDtoValidator()
        {
            RuleFor(c => c.DiarioBordoId)
                .NotEmpty()
                .WithMessage("O Id do diario de bordo deve ser informado");

            RuleFor(c => c.UsuarioId)
                .NotEmpty()
                .WithMessage("O id do usuario deve ser informado");           
        }
    }    
}
