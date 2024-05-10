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
    public class AlterarConsolidacaoFrequenciaAlunoMensalCommandHandler : IRequestHandler<AlterarConsolidacaoFrequenciaAlunoMensalCommand, bool>
    {
        private readonly IRepositorioConsolidacaoFrequenciaAlunoMensal repositorioConsolidacaoFrequenciaAlunoMensal;

        public AlterarConsolidacaoFrequenciaAlunoMensalCommandHandler(IRepositorioConsolidacaoFrequenciaAlunoMensal repositorioConsolidacaoFrequenciaAlunoMensal)
        {
            this.repositorioConsolidacaoFrequenciaAlunoMensal = repositorioConsolidacaoFrequenciaAlunoMensal ?? throw new ArgumentNullException(nameof(repositorioConsolidacaoFrequenciaAlunoMensal));
        }
        public async Task<bool> Handle(AlterarConsolidacaoFrequenciaAlunoMensalCommand request, CancellationToken cancellationToken)
        {
            await repositorioConsolidacaoFrequenciaAlunoMensal.AlterarConsolidacaoAluno(request.ConsolidacaoId, request.Percentual, request.QuantidadeAulas, request.QuantidadeAusencias, request.QuantidadeCompensacoes);
            return true;
        }
    }
}
