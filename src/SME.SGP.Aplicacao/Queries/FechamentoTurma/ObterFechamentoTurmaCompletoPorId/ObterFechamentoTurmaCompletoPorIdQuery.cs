using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoTurmaCompletoPorIdQuery : IRequest<FechamentoTurma>
    {
        public ObterFechamentoTurmaCompletoPorIdQuery(long fechamentoTurmaId)
        {
            FechamentoTurmaId = fechamentoTurmaId;
        }

        public long FechamentoTurmaId { get; set; }
    }

    public class ObterFechamentoTurmaCompletoPorIdQueryValidator : AbstractValidator<ObterFechamentoTurmaPorIdQuery>
    {
        public ObterFechamentoTurmaCompletoPorIdQueryValidator()
        {
            RuleFor(a => a.FechamentoTurmaId)
                .NotNull()
                .WithMessage("Necessário informar o id para obter o fechamento da turma");
        }
    }
}