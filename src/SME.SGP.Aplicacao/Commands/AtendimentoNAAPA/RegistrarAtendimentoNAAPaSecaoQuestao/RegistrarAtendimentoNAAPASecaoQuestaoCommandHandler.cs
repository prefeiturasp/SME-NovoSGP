using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands
{
    public class RegistrarAtendimentoNAAPASecaoQuestaoCommandHandler : IRequestHandler<RegistrarAtendimentoNAAPASecaoQuestaoCommand, long>
    {
        private readonly IRepositorioQuestaoAtendimentoNAAPA repositorioQuestaoEncaminhamentoNAAPA;

        public RegistrarAtendimentoNAAPASecaoQuestaoCommandHandler(IRepositorioQuestaoAtendimentoNAAPA repositorioQuestaoEncaminhamentoNAAPA)
        {
            this.repositorioQuestaoEncaminhamentoNAAPA = repositorioQuestaoEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioQuestaoEncaminhamentoNAAPA));
        }

        public async Task<long> Handle(RegistrarAtendimentoNAAPASecaoQuestaoCommand request, CancellationToken cancellationToken)
        {
            var questao = MapearParaEntidade(request);
            return await repositorioQuestaoEncaminhamentoNAAPA.SalvarAsync(questao);
        }

        private QuestaoEncaminhamentoNAAPA MapearParaEntidade(RegistrarAtendimentoNAAPASecaoQuestaoCommand request)
            => new QuestaoEncaminhamentoNAAPA()
            {
                QuestaoId = request.QuestaoId,
                EncaminhamentoNAAPASecaoId = request.SecaoId
            };
    }
}
