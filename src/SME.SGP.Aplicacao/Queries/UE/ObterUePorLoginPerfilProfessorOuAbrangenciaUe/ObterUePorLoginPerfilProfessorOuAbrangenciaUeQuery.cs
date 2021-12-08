using FluentValidation;
using MediatR;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterUePorLoginPerfilProfessorOuAbrangenciaUeQuery : IRequest<string>
    {
        public string Login { get; set; }
        public Guid Perfil { get; set; }

        public ObterUePorLoginPerfilProfessorOuAbrangenciaUeQuery(string login, Guid perfil)
        {
            Login = login;
            Perfil = perfil;
        }
    }

    public class ObterUePorLoginPerfilProfessorOuAbrangenciaUeQueryValidator : AbstractValidator<ObterUePorLoginPerfilProfessorOuAbrangenciaUeQuery>
    {
        public ObterUePorLoginPerfilProfessorOuAbrangenciaUeQueryValidator()
        {
            RuleFor(c => c.Login)
            .NotEmpty()
            .WithMessage("O login deve ser informado para consulta da UE.");

            RuleFor(c => c.Perfil)
            .NotEmpty()
            .WithMessage("O perrfil deve ser informado para consulta da UE.");
        }
    }
}
