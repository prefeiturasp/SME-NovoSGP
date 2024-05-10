using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;

namespace SME.SGP.Aplicacao
{
    public class AlterarSenhaComTokenRecuperacaoCommand : IRequest<string>
    {
        public AlterarSenhaComTokenRecuperacaoCommand(Guid token, string senha)
        {
            Token = token;
            Senha = senha;
        }

        public Guid Token { get; }
        public string Senha { get; }
    }

    public class AlterarSenhaComTokenRecuperacaoCommandValidator : AbstractValidator<AlterarSenhaComTokenRecuperacaoCommand>
    {
        public AlterarSenhaComTokenRecuperacaoCommandValidator()
        {
            RuleFor(a => a.Token)
                .NotEmpty()
                .WithMessage("O token de recuperação de senha deve ser informado para alteração da senha");

            RuleFor(a => a.Senha)
                .NotEmpty()
                .WithMessage("A nova senha deve ser informada para alteração");

        }
    }
}
