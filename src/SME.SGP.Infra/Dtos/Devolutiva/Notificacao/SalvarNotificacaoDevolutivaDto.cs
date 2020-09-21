using FluentValidation;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;


namespace SME.SGP.Infra.Dtos
{
    public class SalvarNotificacaoDevolutivaDto
    {
        public SalvarNotificacaoDevolutivaDto(Turma turma, Usuario usuario, long devolutivaId)
        {
            Turma = turma;
            Usuario = usuario;
            DevolutivaId = devolutivaId;
        }

        public Turma Turma { get; set; }
        public Usuario Usuario { get; set; }

        public long DevolutivaId;
    }

    public class SalvarNotificacaoDevolutivaDtoValidator : AbstractValidator<SalvarNotificacaoDevolutivaDto>
    {
        public SalvarNotificacaoDevolutivaDtoValidator()
        {
            RuleFor(c => c.Turma)
                .NotEmpty()
                .WithMessage("A turma deve ser informada.");

            RuleFor(c => c.Usuario)
                .NotEmpty()
                .WithMessage("O usuário deve ser informado.");

            RuleFor(c => c.DevolutivaId)
                .NotEmpty()
                .WithMessage("A Devolutiva deve ser informada.");

            RuleFor(c => c.Turma.Ue)
                .NotEmpty()
                .WithMessage("A UE deve ser informada.");

            RuleFor(c => c.Turma.Ue.Dre)
                .NotEmpty()
                .WithMessage("A Dre deve ser informada.");
        }
    }
}
