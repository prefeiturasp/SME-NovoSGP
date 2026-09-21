using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoAlunoPorDisciplinaIdQuery : IRequest<IEnumerable<FechamentoAluno>>
    {
        public ObterFechamentoAlunoPorDisciplinaIdQuery(long fechamentoTurmaDisciplinaId)
        {
            FechamentoTurmaDisciplinaId = fechamentoTurmaDisciplinaId;
        }

        public long FechamentoTurmaDisciplinaId { get; set; }
    }

    public class ObterFechamentoAlunoPorDisciplinaIdQueryValidator : AbstractValidator<ObterFechamentoAlunoPorDisciplinaIdQuery>
    {
        public ObterFechamentoAlunoPorDisciplinaIdQueryValidator()
        {
            RuleFor(a => a.FechamentoTurmaDisciplinaId)
                .NotEmpty()
                .WithMessage("Necessário informar o Id da disciplina");
        }
    }
}
