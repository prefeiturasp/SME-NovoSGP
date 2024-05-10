using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterParecerConclusivoEmAprovacaoPorConselhoClasseAlunoQuery : IRequest<IEnumerable<WFAprovacaoParecerConclusivo>>
    {
        public ObterParecerConclusivoEmAprovacaoPorConselhoClasseAlunoQuery(long conselhoClasseAlunoId)
        {
            ConselhoClasseAlunoId = conselhoClasseAlunoId;
        }

        public long ConselhoClasseAlunoId { get; }
    }

    public class ObterParecerConclusivoEmAprovacaoPorConselhoClasseAlunoQueryValidator : AbstractValidator<ObterParecerConclusivoEmAprovacaoPorConselhoClasseAlunoQuery>
    {
        public ObterParecerConclusivoEmAprovacaoPorConselhoClasseAlunoQueryValidator()
        {
            RuleFor(a => a.ConselhoClasseAlunoId)
                .NotEmpty()
                .WithMessage("O identificador do conselho de classe do aluno deve ser informado para consulta do parecer conclusivo em aprovação");
        }
    }
}
