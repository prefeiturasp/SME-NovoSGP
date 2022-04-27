using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class LimparConsolidacaoFrequenciaAlunoPorTurmasEMesesCommandHandler : IRequestHandler<LimparConsolidacaoFrequenciaAlunoPorTurmasEMesesCommand, bool>
    {
        private readonly IRepositorioConsolidacaoFrequenciaAlunoMensal _repositorioConsolidacaoFrequenciaAlunoMensal;

        public LimparConsolidacaoFrequenciaAlunoPorTurmasEMesesCommandHandler(IRepositorioConsolidacaoFrequenciaAlunoMensal repositorioConsolidacaoFrequenciaAlunoMensal)
        {
            _repositorioConsolidacaoFrequenciaAlunoMensal = repositorioConsolidacaoFrequenciaAlunoMensal ?? throw new System.ArgumentNullException(nameof(repositorioConsolidacaoFrequenciaAlunoMensal));
        }

        public async Task<bool> Handle(LimparConsolidacaoFrequenciaAlunoPorTurmasEMesesCommand request, CancellationToken cancellationToken)
        {
            await _repositorioConsolidacaoFrequenciaAlunoMensal.LimparConsolidacaoFrequenciasAlunosPorTurmasEMeses(request.TurmasIds, request.Meses);
            return true;
        }
    }
}
