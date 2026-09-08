using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class BloquearConsolidacaoFrequenciaAlunoMensalPorTurmaEMesCommandHandler : IRequestHandler<BloquearConsolidacaoFrequenciaAlunoMensalPorTurmaEMesCommand, bool>
    {
        private readonly IRepositorioConsolidacaoFrequenciaAlunoMensal repositorioConsolidacaoFrequenciaAlunoMensal;

        public BloquearConsolidacaoFrequenciaAlunoMensalPorTurmaEMesCommandHandler(IRepositorioConsolidacaoFrequenciaAlunoMensal repositorioConsolidacaoFrequenciaAlunoMensal)
        {
            this.repositorioConsolidacaoFrequenciaAlunoMensal = repositorioConsolidacaoFrequenciaAlunoMensal ?? throw new ArgumentNullException(nameof(repositorioConsolidacaoFrequenciaAlunoMensal));
        }

        public async Task<bool> Handle(BloquearConsolidacaoFrequenciaAlunoMensalPorTurmaEMesCommand request, CancellationToken cancellationToken)
        {
            await repositorioConsolidacaoFrequenciaAlunoMensal.BloquearConsolidacaoFrequenciaAlunoMensalPorTurmaEMes(request.TurmaId, request.Mes);
            return true;
        }
    }
}
