using FluentValidation;
using MediatR;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoTurmaDisciplinaPorTurmaIdQuery : IRequest<IEnumerable<TurmaFechamentoDisciplinaSituacaoDto>>
    {
        public long TurmaId { get; set; }

        public ObterFechamentoTurmaDisciplinaPorTurmaIdQuery(long turmaId)
        {
            TurmaId = turmaId;
        }
    }

    public class ObterFechamentoTurmaDisciplinaPorTurmaIdQueryValidator : AbstractValidator<ObterFechamentoTurmaDisciplinaPorTurmaIdQuery>
    {
        public ObterFechamentoTurmaDisciplinaPorTurmaIdQueryValidator()
        {
            RuleFor(a => a.TurmaId)
                .NotEmpty()
                .WithMessage("É necessário informar o id da turma para obter o fechamento da turma por disciplina");
        }
    }
}
