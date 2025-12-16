using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.NovoEncaminhamentoNAAPA.ExcluirNovoEncaminhamentoNAAPA
{
    public class ExcluirNovoEncaminhamentoNAAPACommand : IRequest<bool>
    {
        public ExcluirNovoEncaminhamentoNAAPACommand(long encaminhamentoNAAPAId)
        {
            EncaminhamentoNAAPAId = encaminhamentoNAAPAId;
        }

        public long EncaminhamentoNAAPAId { get; }
    }

    public class ExcluirNovoEncaminhamentoNAAPACommandValidator : AbstractValidator<ExcluirNovoEncaminhamentoNAAPACommand>
    {
        public ExcluirNovoEncaminhamentoNAAPACommandValidator()
        {
            RuleFor(c => c.EncaminhamentoNAAPAId)
            .NotEmpty()
            .WithMessage("O id do atendimento NAAPA deve ser informado para exclusão.");
        }
    }
}