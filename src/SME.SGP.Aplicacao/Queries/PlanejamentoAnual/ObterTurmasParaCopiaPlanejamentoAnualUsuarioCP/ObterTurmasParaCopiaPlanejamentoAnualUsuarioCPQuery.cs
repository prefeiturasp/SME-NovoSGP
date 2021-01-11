using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasParaCopiaPlanejamentoAnualUsuarioCPQuery : IRequest<IEnumerable<TurmaParaCopiaPlanoAnualDto>>
    {
        public ObterTurmasParaCopiaPlanejamentoAnualUsuarioCPQuery(long turmaId, bool ensinoEspecial, bool consideraHistorico)
        {
            TurmaId = turmaId;
            EnsinoEspecial = ensinoEspecial;
            ConsideraHistorico = consideraHistorico;
        }

        public long TurmaId { get; set; }
        public bool EnsinoEspecial { get; set; }
        public bool ConsideraHistorico { get; set; }
    }

    public class ObterTurmasParaCopiaPlanejamentoAnualQueryCPValidator : AbstractValidator<ObterTurmasParaCopiaPlanejamentoAnualUsuarioCPQuery>
    {
        public ObterTurmasParaCopiaPlanejamentoAnualQueryCPValidator()
        {
            RuleFor(c => c.TurmaId)
                .NotEmpty()
                .WithMessage("A turma deve ser informada.");

        }
    }
}
