using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ExcluirCompensacaoAusenciaPorAulaIdCommand : IRequest<bool>
    {
        public ExcluirCompensacaoAusenciaPorAulaIdCommand(long aulaId)
        {
            AulaId = aulaId;
        }

        public long AulaId { get; set; }
    }
    
    public class ExcluirCompensacaoAusenciaPorAulaIdCommandValidator : AbstractValidator<ExcluirCompensacaoAusenciaPorAulaIdCommand>
    {
        public ExcluirCompensacaoAusenciaPorAulaIdCommandValidator()
        {
            RuleFor(c => c.AulaId)
                .NotNull()
                .WithMessage("O id da aula deve ser informado para efetuar a exclusão de compensações.");
        }
    }
}
