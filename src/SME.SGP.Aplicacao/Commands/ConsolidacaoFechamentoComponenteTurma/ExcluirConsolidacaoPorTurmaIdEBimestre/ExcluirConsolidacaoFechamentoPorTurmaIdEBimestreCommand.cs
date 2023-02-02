using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirConsolidacaoFechamentoPorTurmaIdEBimestreCommand : IRequest
    {
        public long TurmaId { get; set; }
        public int Bimestre { get; set; }

        public ExcluirConsolidacaoFechamentoPorTurmaIdEBimestreCommand(long turmaId, int bimestre)
        {
            TurmaId = turmaId;
            Bimestre = bimestre;
        }
    }

    public class ExcluirConsolidacaoPorTurmaIdEBimestreCommandValidator : AbstractValidator<ExcluirConsolidacaoFechamentoPorTurmaIdEBimestreCommand>
    {
        public ExcluirConsolidacaoPorTurmaIdEBimestreCommandValidator()
        {
            RuleFor(a => a.TurmaId)
                .NotEmpty()
                .WithMessage("Será necessário informar o id da turma para realizar a exclusão dos registros de fechamamentos consolidados");

            RuleFor(a => a.Bimestre)
               .NotEmpty()
               .WithMessage("Será necessário informar o bimestre para realizar a exclusão dos registros de fechamamentos consolidados");
        }
    }
}
