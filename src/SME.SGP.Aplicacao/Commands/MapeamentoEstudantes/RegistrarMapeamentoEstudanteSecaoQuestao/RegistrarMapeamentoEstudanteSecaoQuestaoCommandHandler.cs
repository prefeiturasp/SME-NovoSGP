using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands
{
    public class RegistrarMapeamentoEstudanteSecaoQuestaoCommandHandler : IRequestHandler<RegistrarMapeamentoEstudanteSecaoQuestaoCommand, long>
    {
        private readonly IRepositorioQuestaoMapeamentoEstudante repositorioQuestao;

        public RegistrarMapeamentoEstudanteSecaoQuestaoCommandHandler(IRepositorioQuestaoMapeamentoEstudante repositorioQuestao)
        {
            this.repositorioQuestao = repositorioQuestao ?? throw new ArgumentNullException(nameof(repositorioQuestao));
        }

        public async Task<long> Handle(RegistrarMapeamentoEstudanteSecaoQuestaoCommand request, CancellationToken cancellationToken)
        {
            var questao = MapearParaEntidade(request);
            return await repositorioQuestao.SalvarAsync(questao);
        }

        private QuestaoMapeamentoEstudante MapearParaEntidade(RegistrarMapeamentoEstudanteSecaoQuestaoCommand request)
            => new QuestaoMapeamentoEstudante()
            {
                QuestaoId = request.QuestaoId,
                MapeamentoEstudanteSecaoId = request.SecaoId
            };
    }
}
