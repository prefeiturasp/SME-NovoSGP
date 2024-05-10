using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarQuestaoEncaminhamentoNAAPACommandHandler : IRequestHandler<AlterarQuestaoEncaminhamentoNAAPACommand, bool>
    {
        private readonly IRepositorioQuestaoEncaminhamentoNAAPA repositorioQuestaoEncaminhamentoNAAPA;

        public AlterarQuestaoEncaminhamentoNAAPACommandHandler(IRepositorioQuestaoEncaminhamentoNAAPA repositorioQuestaoEncaminhamentoNAAPA)
        {
            this.repositorioQuestaoEncaminhamentoNAAPA = repositorioQuestaoEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioQuestaoEncaminhamentoNAAPA));
        }

        public async Task<bool> Handle(AlterarQuestaoEncaminhamentoNAAPACommand request, CancellationToken cancellationToken)
        {
            await repositorioQuestaoEncaminhamentoNAAPA.SalvarAsync(request.Questao);
            return true;
        }
    }
}
