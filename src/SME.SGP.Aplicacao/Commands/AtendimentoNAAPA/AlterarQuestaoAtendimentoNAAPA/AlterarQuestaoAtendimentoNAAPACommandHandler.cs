using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarQuestaoAtendimentoNAAPACommandHandler : IRequestHandler<AlterarQuestaoAtendimentoNAAPACommand, bool>
    {
        private readonly IRepositorioQuestaoEncaminhamentoNAAPA repositorioQuestaoEncaminhamentoNAAPA;

        public AlterarQuestaoAtendimentoNAAPACommandHandler(IRepositorioQuestaoEncaminhamentoNAAPA repositorioQuestaoEncaminhamentoNAAPA)
        {
            this.repositorioQuestaoEncaminhamentoNAAPA = repositorioQuestaoEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioQuestaoEncaminhamentoNAAPA));
        }

        public async Task<bool> Handle(AlterarQuestaoAtendimentoNAAPACommand request, CancellationToken cancellationToken)
        {
            await repositorioQuestaoEncaminhamentoNAAPA.SalvarAsync(request.Questao);
            return true;
        }
    }
}
