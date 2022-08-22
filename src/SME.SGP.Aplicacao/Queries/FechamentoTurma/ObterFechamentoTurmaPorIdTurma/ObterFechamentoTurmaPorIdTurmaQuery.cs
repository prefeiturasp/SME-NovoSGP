using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoTurmaPorIdTurmaQuery : IRequest<FechamentoTurma>
    {
        public ObterFechamentoTurmaPorIdTurmaQuery(long turmaId, int? bimestre = 0)
        {
            TurmaId = turmaId;
            Bimestre = bimestre;
        }

        public long TurmaId { get; set; }
        public int? Bimestre { get; set; }
    }

    public class ObterFechamentoTurmaPorIdTurmaQueryValidator : AbstractValidator<ObterFechamentoTurmaPorIdTurmaQuery>
    {
        public ObterFechamentoTurmaPorIdTurmaQueryValidator()
        {
            RuleFor(a => a.TurmaId)
                .NotNull()
                .WithMessage("Necessário informar o id da turma para obter o fechamento da turma");
        }
    }
}