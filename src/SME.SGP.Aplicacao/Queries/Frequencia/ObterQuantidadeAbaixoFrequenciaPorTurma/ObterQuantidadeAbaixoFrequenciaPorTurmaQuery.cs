using MediatR;
using SME.SGP.Infra.Dtos.Frequencia;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.Frequencia.ObterFrequenciaPorLimitePercentual
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