using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExcluirSecoesEncaminhamentoNAAPAPorEncaminhamentoIdCommand : IRequest<bool>
    {
        public ExcluirSecoesEncaminhamentoNAAPAPorEncaminhamentoIdCommand(long encaminhamentoNAAPAId)
        {
            EncaminhamentoNAAPAId = encaminhamentoNAAPAId;
        }

        public long EncaminhamentoNAAPAId { get; }
    }

    public class ExcluirSecoesEncaminhamentoNAAPAPorEncaminhamentoIdCommandValidator : AbstractValidator<ExcluirSecoesEncaminhamentoNAAPAPorEncaminhamentoIdCommand>
    {
        public ExcluirSecoesEncaminhamentoNAAPAPorEncaminhamentoIdCommandValidator()
        {
            RuleFor(c => c.EncaminhamentoNAAPAId)
            .NotEmpty()
            .WithMessage("O id do encaminhamento NAAPA deve ser informado para exclusão de suas seções.");

        }
    }
}
