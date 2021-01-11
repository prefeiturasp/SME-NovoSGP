using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExcluirSecoesEncaminhamentoAEEPorEncaminhamentoIdCommand : IRequest<bool>
    {
        public ExcluirSecoesEncaminhamentoAEEPorEncaminhamentoIdCommand(long encaminhamentoAEEId)
        {
            EncaminhamentoAEEId = encaminhamentoAEEId;
        }

        public long EncaminhamentoAEEId { get; }
    }

    public class ExcluirSecoesEncaminhamentoAEEPorEncaminhamentoIdCommandValidator : AbstractValidator<ExcluirSecoesEncaminhamentoAEEPorEncaminhamentoIdCommand>
    {
        public ExcluirSecoesEncaminhamentoAEEPorEncaminhamentoIdCommandValidator()
        {
            RuleFor(c => c.EncaminhamentoAEEId)
            .NotEmpty()
            .WithMessage("O id do encaminhamento AEE deve ser informado para exclusão de suas seções.");

        }
    }
}
