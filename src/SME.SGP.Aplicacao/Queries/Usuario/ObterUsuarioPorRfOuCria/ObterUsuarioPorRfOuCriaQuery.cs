using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioPorRfOuCriaQuery : IRequest<Usuario>
    {
        public ObterUsuarioPorRfOuCriaQuery(string usuarioRf)
        {
            UsuarioRf = usuarioRf;
        }

        public string UsuarioRf { get; set; }
    }

    public class ObterUsuarioPorRfOuCriaQueryValidator : AbstractValidator<ObterUsuarioPorRfOuCriaQuery>
    {
        public ObterUsuarioPorRfOuCriaQueryValidator()
        {
            RuleFor(c => c.UsuarioRf)
               .NotEmpty()
               .WithMessage("O rf do usuário deve ser informado para pesquisa do mesmo.");
        }
    }
}
