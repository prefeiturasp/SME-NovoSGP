using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands
{
    public class RegistrarEncaminhamentoAEESecaoQuestaoCommandHandler : IRequestHandler<RegistrarEncaminhamentoAEESecaoQuestaoCommand, long>
    {
        private readonly IRepositorioQuestaoEncaminhamentoAEE repositorioQuestaoEncaminhamentoAEE;

        public RegistrarEncaminhamentoAEESecaoQuestaoCommandHandler(IRepositorioQuestaoEncaminhamentoAEE repositorioQuestaoEncaminhamentoAEE)
        {
            this.repositorioQuestaoEncaminhamentoAEE = repositorioQuestaoEncaminhamentoAEE ?? throw new ArgumentNullException(nameof(repositorioQuestaoEncaminhamentoAEE));
        }

        public async Task<long> Handle(RegistrarEncaminhamentoAEESecaoQuestaoCommand request, CancellationToken cancellationToken)
        {
            var questao = MapearParaEntidade(request);
            var id = await repositorioQuestaoEncaminhamentoAEE.SalvarAsync(questao);
            return id;
        }

        private QuestaoEncaminhamentoAEE MapearParaEntidade(RegistrarEncaminhamentoAEESecaoQuestaoCommand request)
            => new QuestaoEncaminhamentoAEE()
            {
                QuestaoId = request.QuestaoId,
                EncaminhamentoAEESecaoId = request.SecaoId
            };
    }
}
