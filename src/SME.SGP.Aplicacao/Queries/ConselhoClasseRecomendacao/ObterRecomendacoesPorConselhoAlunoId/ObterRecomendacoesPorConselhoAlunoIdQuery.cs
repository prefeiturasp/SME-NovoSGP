using FluentValidation;
using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterRecomendacoesPorConselhoAlunoIdQuery : IRequest<IEnumerable<long>>
    {
        public long ConselhoClasseAlunoId { get; set; }

        public ObterRecomendacoesPorConselhoAlunoIdQuery(long conselhoClasseAlunoId)
        {
            ConselhoClasseAlunoId = conselhoClasseAlunoId;
        }
    }

    public class ObterRecomendacoesPorConselhoAlunoIdQueryValidator : AbstractValidator<ObterRecomendacoesPorConselhoAlunoIdQuery>
    {
        public ObterRecomendacoesPorConselhoAlunoIdQueryValidator()
        {
            RuleFor(a => a.ConselhoClasseAlunoId)
                .NotEmpty()
                .WithMessage("É necessário informar o id do conselho de classe do estudante para consultar as recomendações associadas ao mesmo.");
        }
    }
}
