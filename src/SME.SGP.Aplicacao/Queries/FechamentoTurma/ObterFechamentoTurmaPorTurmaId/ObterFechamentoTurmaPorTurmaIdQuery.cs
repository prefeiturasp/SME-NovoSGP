using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoTurmaPorTurmaIdQuery : IRequest<FechamentoTurma>
    {
        public ObterFechamentoTurmaPorTurmaIdQuery(long fechamentoTurmaId)
        {
            FechamentoTurmaId = fechamentoTurmaId;
        }

        public long FechamentoTurmaId { get; set; }
    }

    public class ObterFechamentoTurmaPorTurmaIdQueryValidator : AbstractValidator<ObterFechamentoTurmaPorTurmaIdQuery>
    {
        public ObterFechamentoTurmaPorTurmaIdQueryValidator()
        {
            RuleFor(a => a.FechamentoTurmaId)
                .NotNull()
                .WithMessage("Necessário informar o id para obter o fechamento da turma");
        }
    }
}
