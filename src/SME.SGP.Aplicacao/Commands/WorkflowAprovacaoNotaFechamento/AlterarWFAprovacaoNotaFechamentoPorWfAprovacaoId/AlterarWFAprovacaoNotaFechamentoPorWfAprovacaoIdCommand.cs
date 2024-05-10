using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarWFAprovacaoNotaFechamentoPorWfAprovacaoIdCommand : IRequest<bool>
    {
        public long WorkflowAprovacaoId { get; set; }
        public long[] WorkflowAprovacaoFechamentoNotaIds { get; set; }
    }

    public class AlterarWFAprovacaoNotaFechamentoPorWfAprovacaoIdCommandValidator : AbstractValidator<AlterarWFAprovacaoNotaFechamentoPorWfAprovacaoIdCommand>
    {
        public AlterarWFAprovacaoNotaFechamentoPorWfAprovacaoIdCommandValidator()
        {
            RuleFor(a => a.WorkflowAprovacaoId)
                 .NotEmpty().WithMessage("É necessário informar o id do workflow de aprovação para poder fazer o relacionamento com o workflow de fechamento nota final");

            RuleFor(a => a.WorkflowAprovacaoId)
                 .NotEmpty().WithMessage("É necessário informar os ids dos workflows de fechamento nota final para inserir o relacionamento com a entidade principal de workflow aprovação");
        }
    }
}
