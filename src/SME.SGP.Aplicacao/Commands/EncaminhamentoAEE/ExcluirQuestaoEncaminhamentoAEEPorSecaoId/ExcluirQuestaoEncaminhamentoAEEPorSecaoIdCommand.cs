using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExcluirQuestaoEncaminhamentoAEEPorSecaoIdCommand : IRequest<bool>
    {
        public ExcluirQuestaoEncaminhamentoAEEPorSecaoIdCommand(long encaminhamentoAEESecaoId)
        {
            EncaminhamentoAEESecaoId = encaminhamentoAEESecaoId;
        }

        public long EncaminhamentoAEESecaoId { get; }
    }

    public class ExcluirQuestaoEncaminhamentoAEEPorSecaoIdCommandValidator : AbstractValidator<ExcluirQuestaoEncaminhamentoAEEPorSecaoIdCommand>
    {
        public ExcluirQuestaoEncaminhamentoAEEPorSecaoIdCommandValidator()
        {
            RuleFor(c => c.EncaminhamentoAEESecaoId)
            .NotEmpty()
            .WithMessage("O id da seção do encaminhamento AEE deve ser informado para exclusão de suas questões.");
        }
    }
}
