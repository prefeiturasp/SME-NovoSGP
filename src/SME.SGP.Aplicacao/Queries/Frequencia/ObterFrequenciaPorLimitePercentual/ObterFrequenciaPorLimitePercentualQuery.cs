using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.Frequencia.ObterFrequenciaPorLimitePercentual
{
    public class ObterFrequenciaPorLimitePercentualQuery : IRequest<IEnumerable<ConsolidacaoFrequenciaAlunoMensalDto>>
    {
        public int AnoLetivo { get; set; }
        public double LimitePercentual { get; set; }

        public ObterFrequenciaPorLimitePercentualQuery(int anoLetivo, double limitePercentual)
        {
            AnoLetivo = anoLetivo;
            LimitePercentual = limitePercentual;
        }
    }
}