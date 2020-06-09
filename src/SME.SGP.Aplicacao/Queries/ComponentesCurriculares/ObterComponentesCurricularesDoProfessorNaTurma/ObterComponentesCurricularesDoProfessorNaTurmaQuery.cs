using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesDoProfessorNaTurmaQuery : IRequest<IEnumerable<ComponenteCurricularEol>>
    {
        public ObterComponentesCurricularesDoProfessorNaTurmaQuery(string codigoTurma, string login, Guid perfilUsuario)
        {
            CodigoTurma = codigoTurma;
            Login = login;
            PerfilUsuario = perfilUsuario;
        }

        public string CodigoTurma { get; }
        public string Login { get; }
        public Guid PerfilUsuario { get; }
        public bool EhProfessorCJ { get; }
    }

    public class ObterComponentesCurricularesDoProfessorNaTurmaQueryValidator : AbstractValidator<ObterComponentesCurricularesDoProfessorNaTurmaQuery>
    {
        public ObterComponentesCurricularesDoProfessorNaTurmaQueryValidator()
        {
            RuleFor(c => c.CodigoTurma)
                .NotEmpty()
                .WithMessage("O nome da turma deve ser informado.");

            RuleFor(c => c.Login)
                .NotEmpty()
                .WithMessage("O login do usuário deve ser informado.");


            RuleFor(c => c.PerfilUsuario)
                .NotEmpty()
                .WithMessage("O perfil do usuário deve ser informado.");
        }
    }
}
