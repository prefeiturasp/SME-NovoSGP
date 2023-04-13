using FluentValidation;
using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasComDevolutivaPorAulaTurmaIdQuery : IRequest<IEnumerable<long>>
    {
        public ObterTurmasComDevolutivaPorAulaTurmaIdQuery(string aulaTurmaId)
        {
            AulaTurmaId = aulaTurmaId;
        }

        public string AulaTurmaId { get; set; }

        public class ObterTurmasComDevolutivaPorAulaTurmaIdQueryyValidator : AbstractValidator<ObterTurmasComDevolutivaPorAulaTurmaIdQuery>
        {
            public ObterTurmasComDevolutivaPorAulaTurmaIdQueryyValidator()
            {
                RuleFor(c => c.AulaTurmaId)
                .NotEmpty()
                .WithMessage("O Código da Turma deve ser informado para consulta de turmas com devolutivas.");

            }
        }
    }
}
