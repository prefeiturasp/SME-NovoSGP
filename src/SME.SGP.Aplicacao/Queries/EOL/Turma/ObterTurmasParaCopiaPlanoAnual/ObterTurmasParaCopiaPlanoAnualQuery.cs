using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasParaCopiaPlanoAnualQuery : IRequest<IEnumerable<TurmaParaCopiaPlanoAnualDto>>
    {
        public ObterTurmasParaCopiaPlanoAnualQuery(string codigoRF, long componenteCurricular, int turmaId)
        {
            CodigoRf = codigoRF;
            ComponenteCurricular = componenteCurricular;
            TurmaId = turmaId;
        }

        public string CodigoRf { get; set; }
        public long ComponenteCurricular { get; set; }
        public long TurmaId { get; set; }
    }

    public class ObterTurmasParaCopiaPlanoAnualQueryValidator : AbstractValidator<ObterTurmasParaCopiaPlanoAnualQuery>
    {
        public ObterTurmasParaCopiaPlanoAnualQueryValidator()
        {
            RuleFor(c => c.CodigoRf)
            .NotEmpty()
            .WithMessage("O código Rf deve ser informado para copiar plano anual.");
            
            RuleFor(c => c.ComponenteCurricular)
                .GreaterThan(0)
                .WithMessage("O código do componente curricular deve ser informado para para copiar plano anual.");
            
            RuleFor(c => c.TurmaId)
                .GreaterThan(0)
                .WithMessage("O código da turma deve ser informado para copiar plano anual.");
        }
    }
}
