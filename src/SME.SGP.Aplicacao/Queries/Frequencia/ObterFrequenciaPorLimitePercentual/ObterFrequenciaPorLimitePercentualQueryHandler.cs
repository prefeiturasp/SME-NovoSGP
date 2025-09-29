using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.Frequencia.ObterFrequenciaPorLimitePercentual
{
    public class ObterFrequenciaPorLimitePercentualQueryHandler : IRequestHandler<ObterFrequenciaPorLimitePercentualQuery, IEnumerable<ConsolidacaoFrequenciaAlunoMensalDto>>
    {
        private readonly IRepositorioConsolidacaoFrequenciaAlunoMensal repositorioConsolidacaoFrequenciaAlunoMensal;
        public ObterFrequenciaPorLimitePercentualQueryHandler(IRepositorioConsolidacaoFrequenciaAlunoMensal repositorioConsolidacaoFrequenciaAlunoMensal)
        {
            this.repositorioConsolidacaoFrequenciaAlunoMensal = repositorioConsolidacaoFrequenciaAlunoMensal;
        }
        public async Task<IEnumerable<ConsolidacaoFrequenciaAlunoMensalDto>> Handle(ObterFrequenciaPorLimitePercentualQuery request, CancellationToken cancellationToken)
        {
            return await repositorioConsolidacaoFrequenciaAlunoMensal.ObterFrequenciaPorLimitePercentualPorAnoDaTurma(request.AnoLetivo, request.LimitePercentual);
        }
    }
}