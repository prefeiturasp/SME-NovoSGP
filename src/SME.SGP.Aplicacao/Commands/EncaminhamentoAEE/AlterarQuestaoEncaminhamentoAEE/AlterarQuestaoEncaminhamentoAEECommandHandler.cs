using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarQuestaoEncaminhamentoAEECommandHandler : IRequestHandler<AlterarQuestaoEncaminhamentoAEECommand, bool>
    {
        private readonly IRepositorioQuestaoEncaminhamentoAEE repositorioQuestaoEncaminhamentoAEE;

        public AlterarQuestaoEncaminhamentoAEECommandHandler(IRepositorioQuestaoEncaminhamentoAEE repositorioQuestaoEncaminhamentoAEE)
        {
            this.repositorioQuestaoEncaminhamentoAEE = repositorioQuestaoEncaminhamentoAEE ?? throw new ArgumentNullException(nameof(repositorioQuestaoEncaminhamentoAEE));
        }

        public async Task<bool> Handle(AlterarQuestaoEncaminhamentoAEECommand request, CancellationToken cancellationToken)
        {
            await repositorioQuestaoEncaminhamentoAEE.SalvarAsync(request.Questao);
            return true;
        }
    }
}
