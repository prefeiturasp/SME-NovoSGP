using MediatR;
using SME.SGP.Infra.Dtos.ConsolidacaoFrequenciaTurma;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.ConsolidacaoFrequenciaTurma.ObterFrequenciaPorLimitePercentual
{
    public class ObterQuantidadeAbaixoFrequenciaPorTurmaQuery : IRequest<IEnumerable<QuantitativoAlunosFrequenciaBaixaPorTurmaDto>>
    {
        public int AnoLetivo { get; set; }

        public ObterQuantidadeAbaixoFrequenciaPorTurmaQuery(int anoLetivo)
        {
            AnoLetivo = anoLetivo;
        }
    }
}