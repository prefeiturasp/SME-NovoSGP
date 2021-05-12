using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class SalvarFechamentoConsolidadoCommand : IRequest<bool>
    {
        public SalvarFechamentoConsolidadoCommand(long turmaId, int bimestre, long componenteCurricularId)
        {
            TurmaId = turmaId;
            Bimestre = bimestre;
            ComponenteCurricularId = componenteCurricularId;
        }

        public long TurmaId { get; }
        public int Bimestre { get; }
        public long ComponenteCurricularId { get; }
    }

    public class SalvarFechamentoConsolidadoCommandValidator : AbstractValidator<SalvarFechamentoConsolidadoCommand>
    {
        public SalvarFechamentoConsolidadoCommandValidator()
        {
            RuleFor(a => a.TurmaId)
                .NotEmpty()
                .WithMessage("O id do turma deve ser informado para consolidar o fechamento.");

            RuleFor(a => a.Bimestre)
                .NotEmpty()
                .WithMessage("O bimestre deve ser informado para consolidar o fechamento.");

            RuleFor(a => a.ComponenteCurricularId)
               .NotEmpty()
               .WithMessage("O id do componente curricular deve ser informado para consolidar o fechamento.");
        }
    }
}

