using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterConsolidacoesFrequenciaAlunoMensalInsuficientesQueryHandler : IRequestHandler<ObterConsolidacoesFrequenciaAlunoMensalInsuficientesQuery, IEnumerable<ConsolidacaoFreqAlunoMensalInsuficienteDto>>
    {
        private readonly IRepositorioConsolidacaoFrequenciaAlunoMensal repositorioConsolidacaoFrequenciaAlunoMensal;

        public ObterConsolidacoesFrequenciaAlunoMensalInsuficientesQueryHandler(IRepositorioConsolidacaoFrequenciaAlunoMensal repositorioConsolidacaoFrequenciaAlunoMensal)
        {
            this.repositorioConsolidacaoFrequenciaAlunoMensal = repositorioConsolidacaoFrequenciaAlunoMensal ?? throw new ArgumentNullException(nameof(repositorioConsolidacaoFrequenciaAlunoMensal));
        }

        public async Task<IEnumerable<ConsolidacaoFreqAlunoMensalInsuficienteDto>> Handle(ObterConsolidacoesFrequenciaAlunoMensalInsuficientesQuery request, CancellationToken cancellationToken)
        {
            return await repositorioConsolidacaoFrequenciaAlunoMensal.ObterConsolidacoesFrequenciaAlunoMensalInsuficientes(request.UeId, request.AnoLetivo, request.Mes);
        }
    }
}
