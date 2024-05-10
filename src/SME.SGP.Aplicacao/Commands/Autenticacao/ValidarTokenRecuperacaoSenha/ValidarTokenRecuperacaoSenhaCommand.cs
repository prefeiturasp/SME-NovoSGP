using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ValidarTokenRecuperacaoSenhaCommand : IRequest<bool>
    {
        public ValidarTokenRecuperacaoSenhaCommand(Guid token)
        {
            Token = token;
        }

        public Guid Token { get; }
    }

    public class ValidarTokenRecuperacaoSenhaCommandValidator : AbstractValidator<ValidarTokenRecuperacaoSenhaCommand>
    {
        public ValidarTokenRecuperacaoSenhaCommandValidator()
        {
            RuleFor(a => a.Token)
                .NotEmpty()
                .WithMessage("O token de recuperação de senha deve ser informado para validação");
        }
    }
}
