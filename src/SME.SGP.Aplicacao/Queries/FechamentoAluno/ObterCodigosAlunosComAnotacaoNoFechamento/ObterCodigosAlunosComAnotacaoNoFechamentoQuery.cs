using FluentValidation;
using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterCodigosAlunosComAnotacaoNoFechamentoQuery : IRequest<IEnumerable<string>>
    {
        public ObterCodigosAlunosComAnotacaoNoFechamentoQuery(long fechamentoTurmaDisciplinaId)
        {
            FechamentoTurmaDisciplinaId = fechamentoTurmaDisciplinaId;
        }

        public long FechamentoTurmaDisciplinaId { get; }
    }

    public class ObterCodigosAlunosComAnotacaoNoFechamentoQueryValidator : AbstractValidator<ObterCodigosAlunosComAnotacaoNoFechamentoQuery>
    {
        public ObterCodigosAlunosComAnotacaoNoFechamentoQueryValidator()
        {
            RuleFor(a => a.FechamentoTurmaDisciplinaId)
                .NotEmpty()
                .WithMessage("O identificador do fechamento da turma no componente deve ser informado para consulta de anotações dos estudantes");
        }
    }
}
