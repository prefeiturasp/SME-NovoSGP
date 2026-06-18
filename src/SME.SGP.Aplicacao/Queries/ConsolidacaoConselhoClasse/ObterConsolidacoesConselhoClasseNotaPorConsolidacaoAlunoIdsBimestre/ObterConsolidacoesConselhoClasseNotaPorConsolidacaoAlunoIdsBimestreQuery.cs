using FluentValidation;
using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterConsolidacoesConselhoClasseNotaPorConsolidacaoAlunoIdsBimestreQuery : IRequest<IEnumerable<long>>
    {
        public long[] ConsolidacoesAlunoTurmaIds { get; set; }
        public int Bimestre { get; set; }

        public ObterConsolidacoesConselhoClasseNotaPorConsolidacaoAlunoIdsBimestreQuery(long[] consolidacoesIds, int bimestre = 0)
        {
            ConsolidacoesAlunoTurmaIds = consolidacoesIds;
            Bimestre = bimestre;
        }
    }

    public class ObterConsolidacoesConselhoClasseNotaPorConsolidacaoAlunoIdsQueryValidator : AbstractValidator<ObterConsolidacoesConselhoClasseNotaPorConsolidacaoAlunoIdsBimestreQuery>
    {
        public ObterConsolidacoesConselhoClasseNotaPorConsolidacaoAlunoIdsQueryValidator()
        {
            RuleFor(a => a.ConsolidacoesAlunoTurmaIds)
                .NotEmpty()
                .WithMessage("É necessário informar o(s) id(s) da consolidação do aluno para obter as consolidações de nota do conselho do mesmo.");
        }
    }
}
