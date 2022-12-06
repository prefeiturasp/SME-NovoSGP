using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExcluirRespostaEncaminhamentoNAAPACommand : IRequest<bool>
    {
        public ExcluirRespostaEncaminhamentoNAAPACommand(RespostaEncaminhamentoNAAPA resposta)
        {
            Resposta = resposta;
        }

        public RespostaEncaminhamentoNAAPA Resposta { get; }
    }

    public class ExcluirRespostaEncaminhamentoNAAPACommandValidator : AbstractValidator<ExcluirRespostaEncaminhamentoNAAPACommand>
    {
        public ExcluirRespostaEncaminhamentoNAAPACommandValidator()
        {
            RuleFor(c => c.Resposta)
            .NotEmpty()
            .WithMessage("A resposta do encaminhamento naapa deve ser informada para exclusão.");

        }
    }
}
