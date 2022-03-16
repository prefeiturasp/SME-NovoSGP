using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioPorTokenRecuperacaoSenhaQuery : IRequest<Usuario>
    {
        public Guid Token { get; set; }

        public ObterUsuarioPorTokenRecuperacaoSenhaQuery(Guid token)
        {
            Token = token;
        }
    }

    public class ObterUsuarioPorTokenRecuperacaoSenhaQueryValidator : AbstractValidator<ObterUsuarioPorTokenRecuperacaoSenhaQuery>
    {
        public ObterUsuarioPorTokenRecuperacaoSenhaQueryValidator()
        {
            RuleFor(x => x.Token)
                .NotEmpty()
                .WithMessage("O token deve ser informado.");
        }
    }
}