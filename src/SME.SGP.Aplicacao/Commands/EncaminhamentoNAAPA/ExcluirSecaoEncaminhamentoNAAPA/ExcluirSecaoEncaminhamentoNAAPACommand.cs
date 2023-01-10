using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExcluirSecaoEncaminhamentoNAAPACommand : IRequest<bool>
    {
        public ExcluirSecaoEncaminhamentoNAAPACommand(long encaminhamentoNAAPASecaoId)
        {
            EncaminhamentoNAAPASecaoId = encaminhamentoNAAPASecaoId;
        }

        public long EncaminhamentoNAAPASecaoId { get; }
    }

    public class ExcluirSecaoEncaminhamentoNAAPACommandValidator : AbstractValidator<ExcluirSecaoEncaminhamentoNAAPACommand>
    {
        public ExcluirSecaoEncaminhamentoNAAPACommandValidator()
        {
            RuleFor(c => c.EncaminhamentoNAAPASecaoId)
            .NotEmpty()
            .WithMessage("O id da seção do encaminhamento NAAPA deve ser informado para exclusão.");

        }
    }
}
