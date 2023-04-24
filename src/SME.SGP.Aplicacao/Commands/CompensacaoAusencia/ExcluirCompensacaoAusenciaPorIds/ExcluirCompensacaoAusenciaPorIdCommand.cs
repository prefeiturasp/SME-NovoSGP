using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ExcluirCompensacaoAusenciaPorIdsCommand : IRequest<bool>
    {
        public ExcluirCompensacaoAusenciaPorIdsCommand(long[] compensacaoAusenciaIds)
        {
            CompensacaoAusenciaIds = compensacaoAusenciaIds;
        }

        public long[] CompensacaoAusenciaIds { get; set; }
    }
    
    public class ExcluirCompensacaoAusenciaPorIdsCommandValidator : AbstractValidator<ExcluirCompensacaoAusenciaPorIdsCommand>
    {
        public ExcluirCompensacaoAusenciaPorIdsCommandValidator()
        {
            RuleFor(c => c.CompensacaoAusenciaIds)
                .NotNull()
                .WithMessage("Os ids das compensações de ausências deevem ser informados para efetuar a exclusão de compensações que não tem compensações aluno e aula.");
        }
    }
}
