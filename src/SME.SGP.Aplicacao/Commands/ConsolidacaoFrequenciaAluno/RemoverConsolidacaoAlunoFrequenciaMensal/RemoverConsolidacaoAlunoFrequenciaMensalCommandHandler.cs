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
    public class RemoverConsolidacaoAlunoFrequenciaMensalCommandHandler : IRequestHandler<RemoverConsolidacaoAlunoFrequenciaMensalCommand, bool>
    {
        private readonly IRepositorioConsolidacaoFrequenciaAlunoMensal repositorioConsolidacaoFrequenciaAlunoMensal;

        public RemoverConsolidacaoAlunoFrequenciaMensalCommandHandler(IRepositorioConsolidacaoFrequenciaAlunoMensal repositorioConsolidacaoFrequenciaAlunoMensal)
        {
            this.repositorioConsolidacaoFrequenciaAlunoMensal = repositorioConsolidacaoFrequenciaAlunoMensal ?? throw new ArgumentNullException(nameof(repositorioConsolidacaoFrequenciaAlunoMensal));
        }
        public async Task<bool> Handle(RemoverConsolidacaoAlunoFrequenciaMensalCommand request, CancellationToken cancellationToken)
        {
            await repositorioConsolidacaoFrequenciaAlunoMensal.RemoverConsolidacaoAluno(request.ConsolidacaoId);
            return true;
        }
    }
}
