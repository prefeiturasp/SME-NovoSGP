using FluentValidation;
using MediatR;
using SME.SGP.Dto;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterAbrangenciaCompactaVigenteEolPorLoginEPerfilQuery : IRequest<AbrangenciaCompactaVigenteRetornoEOLDTO>
    {
        public ObterAbrangenciaCompactaVigenteEolPorLoginEPerfilQuery(string login, Guid perfil)
        {
            Login = login;
            Perfil = perfil;
        }

        public string Login { get; set; }
        public Guid Perfil { get; set; }
    }
    public class ObterAbrangenciaCompactaVigenteEolPorLoginEPerfilQueryValidator : AbstractValidator<ObterAbrangenciaCompactaVigenteEolPorLoginEPerfilQuery>
    {
        public ObterAbrangenciaCompactaVigenteEolPorLoginEPerfilQueryValidator()
        {
            RuleFor(x => x.Login)
                .NotEmpty()
                .WithMessage("O login do usuário logado deve ser informado.");

            RuleFor(x => x.Perfil)
                .NotEmpty()
                .WithMessage("O perfil do usuário logado deve ser informado.");
        }
    }
}
