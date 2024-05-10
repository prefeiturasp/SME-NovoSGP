using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoTurmaDisciplinaPorIdQuery : IRequest<FechamentoTurmaDisciplina>
    {
        public long Id { get; set; }

        public ObterFechamentoTurmaDisciplinaPorIdQuery(long id)
        {
            Id = id;
        }
    }

    public class ObterFechamentoTurmaDisciplinaPorIdQueryValidator : AbstractValidator<ObterFechamentoTurmaDisciplinaPorIdQuery>
    {
        public ObterFechamentoTurmaDisciplinaPorIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("O id do fechamento turma disciplina deve ser informado.");
        }
    }
}