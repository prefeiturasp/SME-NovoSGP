using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands
{
    public class RegistrarRegistroAcaoSecaoQuestaoCommandHandler : IRequestHandler<RegistrarRegistroAcaoSecaoQuestaoCommand, long>
    {
        private readonly IRepositorioQuestaoRegistroAcaoBuscaAtiva repositorioQuestao;

        public RegistrarRegistroAcaoSecaoQuestaoCommandHandler(IRepositorioQuestaoRegistroAcaoBuscaAtiva repositorioQuestao)
        {
            this.repositorioQuestao = repositorioQuestao ?? throw new ArgumentNullException(nameof(repositorioQuestao));
        }

        public async Task<long> Handle(RegistrarRegistroAcaoSecaoQuestaoCommand request, CancellationToken cancellationToken)
        {
            var questao = MapearParaEntidade(request);
            return await repositorioQuestao.SalvarAsync(questao);
        }

        private QuestaoRegistroAcaoBuscaAtiva MapearParaEntidade(RegistrarRegistroAcaoSecaoQuestaoCommand request)
            => new QuestaoRegistroAcaoBuscaAtiva()
            {
                QuestaoId = request.QuestaoId,
                RegistroAcaoBuscaAtivaSecaoId = request.SecaoId
            };
    }
}
