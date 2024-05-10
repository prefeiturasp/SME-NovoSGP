using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoTurmaPorTurmaIdQuery : IRequest<FechamentoTurma>
    {
        public ObterFechamentoTurmaPorTurmaIdQuery(long turmaId)
        {
            TurmaId = turmaId;
        }

        public long TurmaId { get; set; }
    }

    public class ObterFechamentoTurmaPorTurmaIdQueryValidator : AbstractValidator<ObterFechamentoTurmaPorTurmaIdQuery>
    {
        public ObterFechamentoTurmaPorTurmaIdQueryValidator()
        {
            RuleFor(a => a.TurmaId)
                .NotNull()
                .WithMessage("Necessário informar o id da turma para obter o fechamento da turma");
        }
    }
}
