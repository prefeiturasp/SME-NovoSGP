using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExcluirQuestaoEncaminhamentoNAAPAPorSecaoIdCommand : IRequest<bool>
    {
        public ExcluirQuestaoEncaminhamentoNAAPAPorSecaoIdCommand(long encaminhamentoNAAPASecaoId)
        {
            EncaminhamentoNAAPASecaoId = encaminhamentoNAAPASecaoId;
        }

        public long EncaminhamentoNAAPASecaoId { get; }
    }

    public class ExcluirQuestaoEncaminhamentoNAAPAPorSecaoIdCommandValidator : AbstractValidator<ExcluirQuestaoEncaminhamentoNAAPAPorSecaoIdCommand>
    {
        public ExcluirQuestaoEncaminhamentoNAAPAPorSecaoIdCommandValidator()
        {
            RuleFor(c => c.EncaminhamentoNAAPASecaoId)
            .NotEmpty()
            .WithMessage("O id da seção do encaminhamento NAAPA deve ser informado para exclusão de suas questões.");
        }
    }
}
