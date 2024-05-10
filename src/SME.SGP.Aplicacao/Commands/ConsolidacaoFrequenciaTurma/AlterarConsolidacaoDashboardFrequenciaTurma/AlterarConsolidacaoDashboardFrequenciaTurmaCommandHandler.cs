using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarConsolidacaoDashboardFrequenciaTurmaCommandHandler : IRequestHandler<AlterarConsolidacaoDashboardFrequenciaTurmaCommand, bool>
    {
        private readonly IRepositorioConsolidacaoFrequenciaTurma repositorioConsolidacaoFrequenciaAlunoMensal;

        public AlterarConsolidacaoDashboardFrequenciaTurmaCommandHandler(IRepositorioConsolidacaoFrequenciaTurma repositorioConsolidacaoFrequenciaAlunoMensal)
        {
            this.repositorioConsolidacaoFrequenciaAlunoMensal = repositorioConsolidacaoFrequenciaAlunoMensal ?? throw new ArgumentNullException(nameof(repositorioConsolidacaoFrequenciaAlunoMensal));
        }
        public async Task<bool> Handle(AlterarConsolidacaoDashboardFrequenciaTurmaCommand request, CancellationToken cancellationToken)
        {
            await repositorioConsolidacaoFrequenciaAlunoMensal.AlterarConsolidacaoDashboardTurmaMesPeriodoAno(request.Id, request.QuantidadePresentes, request.QuantidadeAusentes, request.QuantidadeRemotos);
            return true;
        }
    }
}
