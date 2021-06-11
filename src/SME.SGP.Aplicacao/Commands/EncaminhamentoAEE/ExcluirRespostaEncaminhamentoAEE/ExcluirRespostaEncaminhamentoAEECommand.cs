using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExcluirRespostaEncaminhamentoAEECommand : IRequest<bool>
    {
        public ExcluirRespostaEncaminhamentoAEECommand(RespostaEncaminhamentoAEE resposta)
        {
            Resposta = resposta;
        }

        public RespostaEncaminhamentoAEE Resposta { get; }
    }

    public class ExcluirRespostaEncaminhamentoAEECommandValidator : AbstractValidator<ExcluirRespostaEncaminhamentoAEECommand>
    {
        public ExcluirRespostaEncaminhamentoAEECommandValidator()
        {
            RuleFor(c => c.Resposta)
            .NotEmpty()
            .WithMessage("A resposta do encaminhamento deve ser informada para exclusão.");

        }
    }
}
