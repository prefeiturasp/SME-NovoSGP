using FluentValidation;
using MediatR;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAbrangenciaUesPorLoginEPerfilQuery : IRequest<IEnumerable<AbrangenciaUeRetorno>>
    {
        public ObterAbrangenciaUesPorLoginEPerfilQuery(string dreCodigo, string login, Guid perfil)
        {
            DreCodigo = dreCodigo;
            Login = login;
            Perfil = perfil;
        }

        public string DreCodigo { get; set; }
        public string Login { get; set; }
        public Guid Perfil { get; set; }
    }
    public class ObterAbrangenciaUesPorLoginEPerfilQueryValidator : AbstractValidator<ObterAbrangenciaUesPorLoginEPerfilQuery>
    {
        public ObterAbrangenciaUesPorLoginEPerfilQueryValidator()
        {
            RuleFor(x => x.DreCodigo)
                .NotEmpty()
                .WithMessage("O código da Dre deve ser informado.");

            RuleFor(x => x.Login)
                .NotEmpty()
                .WithMessage("O login do usuário logado deve ser informado.");

            RuleFor(x => x.Perfil)
                .NotEmpty()
                .WithMessage("O perfil do usuário logado deve ser informado.");
        }
    }
}
