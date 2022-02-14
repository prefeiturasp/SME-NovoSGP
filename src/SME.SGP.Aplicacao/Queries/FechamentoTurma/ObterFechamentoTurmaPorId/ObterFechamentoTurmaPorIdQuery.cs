using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoTurmaPorIdQuery : IRequest<FechamentoTurma>
    {
        public ObterFechamentoTurmaPorIdQuery(long fechamentoTurmaId)
        {
            FechamentoTurmaId = fechamentoTurmaId;
        }

        public long FechamentoTurmaId { get; set; }
    }

    public class ObterFechamentoTurmaPorIdQueryValidator : AbstractValidator<ObterFechamentoTurmaPorIdQuery>
    {
        public ObterFechamentoTurmaPorIdQueryValidator()
        {
            RuleFor(a => a.FechamentoTurmaId)
                .NotNull()
                .WithMessage("Necessário informar o id para obter o fechamento da turma");
        }
    }
}
