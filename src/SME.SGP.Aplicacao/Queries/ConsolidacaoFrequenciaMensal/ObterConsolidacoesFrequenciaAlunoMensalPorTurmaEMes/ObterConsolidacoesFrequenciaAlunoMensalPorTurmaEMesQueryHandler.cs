using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterConsolidacoesFrequenciaAlunoMensalPorTurmaEMesQueryHandler : IRequestHandler<ObterConsolidacoesFrequenciaAlunoMensalPorTurmaEMesQuery, IEnumerable<ConsolidacaoFrequenciaAlunoMensalDto>>
    {
        private readonly IRepositorioConsolidacaoFrequenciaAlunoMensal repositorioConsolidacaoFrequenciaAlunoMensal;

        public ObterConsolidacoesFrequenciaAlunoMensalPorTurmaEMesQueryHandler(IRepositorioConsolidacaoFrequenciaAlunoMensal repositorioConsolidacaoFrequenciaAlunoMensal)
        {
            this.repositorioConsolidacaoFrequenciaAlunoMensal = repositorioConsolidacaoFrequenciaAlunoMensal ?? throw new ArgumentNullException(nameof(repositorioConsolidacaoFrequenciaAlunoMensal));
        }

        public async Task<IEnumerable<ConsolidacaoFrequenciaAlunoMensalDto>> Handle(ObterConsolidacoesFrequenciaAlunoMensalPorTurmaEMesQuery request, CancellationToken cancellationToken)
        {
            return await repositorioConsolidacaoFrequenciaAlunoMensal.ObterConsolidacoesFrequenciaAlunoMensalPorTurmaEMes(request.TurmaId, request.Mes);
        }
    }
}
