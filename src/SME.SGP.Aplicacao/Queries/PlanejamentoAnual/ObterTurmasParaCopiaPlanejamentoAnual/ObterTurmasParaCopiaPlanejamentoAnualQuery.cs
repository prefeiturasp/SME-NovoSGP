using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasParaCopiaPlanejamentoAnualQuery : IRequest<IEnumerable<TurmaParaCopiaPlanoAnualDto>>
    {
        public ObterTurmasParaCopiaPlanejamentoAnualQuery(long turmaId, long componenteCurricularId, string rf, bool ensinoEspecial)
        {
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
            RF = rf;
            EnsinoEspecial = ensinoEspecial;
        }

        public long TurmaId { get; set; }
        public string RF { get; set; }
        public long ComponenteCurricularId { get; set; }
        public bool EnsinoEspecial { get; set; }
    }

    public class ObterTurmasParaCopiaPlanejamentoAnualQueryValidator : AbstractValidator<ObterTurmasParaCopiaPlanejamentoAnualQuery>
    {
        public ObterTurmasParaCopiaPlanejamentoAnualQueryValidator()
        {
            RuleFor(c => c.TurmaId)
                .NotEmpty()
                .WithMessage("A turma deve ser informada.");

            RuleFor(c => c.RF)
                .NotEmpty()
                .WithMessage("O RF precisa ser informado");

            RuleFor(c => c.ComponenteCurricularId)
                .NotEmpty()
                .WithMessage("O componente curricular deve ser informado.");

        }
    }
}
