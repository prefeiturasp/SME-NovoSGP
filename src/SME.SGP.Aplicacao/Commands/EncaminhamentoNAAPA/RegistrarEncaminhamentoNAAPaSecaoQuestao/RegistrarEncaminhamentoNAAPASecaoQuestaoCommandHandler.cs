using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands
{
    public class RegistrarEncaminhamentoNAAPASecaoQuestaoCommandHandler : IRequestHandler<RegistrarEncaminhamentoNAAPASecaoQuestaoCommand, long>
    {
        private readonly IRepositorioQuestaoEncaminhamentoNAAPA repositorioQuestaoEncaminhamentoNAAPA;

        public RegistrarEncaminhamentoNAAPASecaoQuestaoCommandHandler(IRepositorioQuestaoEncaminhamentoNAAPA repositorioQuestaoEncaminhamentoNAAPA)
        {
            this.repositorioQuestaoEncaminhamentoNAAPA = repositorioQuestaoEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioQuestaoEncaminhamentoNAAPA));
        }

        public async Task<long> Handle(RegistrarEncaminhamentoNAAPASecaoQuestaoCommand request, CancellationToken cancellationToken)
        {
            var questao = MapearParaEntidade(request);
            return await repositorioQuestaoEncaminhamentoNAAPA.SalvarAsync(questao);
        }

        private QuestaoEncaminhamentoNAAPA MapearParaEntidade(RegistrarEncaminhamentoNAAPASecaoQuestaoCommand request)
            => new QuestaoEncaminhamentoNAAPA()
            {
                QuestaoId = request.QuestaoId,
                EncaminhamentoNAAPASecaoId = request.SecaoId
            };
    }
}
