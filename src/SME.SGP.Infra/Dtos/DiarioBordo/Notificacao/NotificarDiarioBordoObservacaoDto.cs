using FluentValidation;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra.Dtos
{
    public class NotificarDiarioBordoObservacaoDto
    {
        public NotificarDiarioBordoObservacaoDto(Turma turma, Usuario usuario)
        {
            Turma = turma;
            Usuario = usuario;
        }

        public Turma Turma { get; set; }
        public Usuario Usuario { get; set; }
    }

    public class NotificarDiarioBordoObservacaoDtoValidator : AbstractValidator<NotificarDiarioBordoObservacaoDto>
    {
        public NotificarDiarioBordoObservacaoDtoValidator()
        {
            RuleFor(c => c.Turma)
                .NotEmpty()
                .WithMessage("A turma deve ser informada.");

            RuleFor(c => c.Usuario)
                .NotEmpty()
                .WithMessage("O usuário deve ser informado.");

            RuleFor(c => c.Turma.Ue)
                .NotEmpty()
                .WithMessage("A UE deve ser informada.");

            RuleFor(c => c.Turma.Ue.Dre)
                .NotEmpty()
                .WithMessage("A Dre deve ser informada.");
        }
    }    
}
