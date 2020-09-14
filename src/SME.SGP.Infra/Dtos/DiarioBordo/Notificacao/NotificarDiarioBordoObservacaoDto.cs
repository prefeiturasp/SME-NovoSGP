using FluentValidation;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra.Dtos
{
    public class NotificarDiarioBordoObservacaoDto
    {
        public NotificarDiarioBordoObservacaoDto(long diarioBordoId, Usuario usuario, long observacaoId)
        {
            DiarioBordoId = diarioBordoId;
            Usuario = usuario;
            ObservacaoId = observacaoId;
        }
        public long ObservacaoId { get; set; }
        public long DiarioBordoId { get; set; }
        public Usuario Usuario { get; set; }
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
