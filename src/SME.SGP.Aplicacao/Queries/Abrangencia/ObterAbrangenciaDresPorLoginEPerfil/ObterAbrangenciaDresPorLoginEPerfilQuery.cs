using FluentValidation;
using MediatR;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAbrangenciaDresPorLoginEPerfilQuery : IRequest<IEnumerable<AbrangenciaDreRetornoDto>>
    {
        public ObterAbrangenciaDresPorLoginEPerfilQuery(string login, Guid perfil)
        {
            Login = login;
            Perfil = perfil;
        }

        public string Login { get; set; }
        public Guid Perfil { get; set; }
    }
    public class ObterAbrangenciaDresPorLoginEPerfilQueryValidator : AbstractValidator<ObterAbrangenciaDresPorLoginEPerfilQuery>
    {
        public ObterAbrangenciaDresPorLoginEPerfilQueryValidator()
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
