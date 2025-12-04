using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestaoTurmasProgramaAtendimentoNAAPAPorIdQueryHandler : ConsultasBase, IRequestHandler<ObterQuestaoTurmasProgramaAtendimentoNAAPAPorIdQuery, QuestaoEncaminhamentoNAAPA>
    {
        private readonly IRepositorioQuestaoEncaminhamentoNAAPA repositorioEncaminhamentoNaapaQuestao;
        public ObterQuestaoTurmasProgramaAtendimentoNAAPAPorIdQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioQuestaoEncaminhamentoNAAPA repositorioEncaminhamentoNaapaQuestao) : base(contextoAplicacao)
        {
            this.repositorioEncaminhamentoNaapaQuestao = repositorioEncaminhamentoNaapaQuestao ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNaapaQuestao));
        }

        public Task<QuestaoEncaminhamentoNAAPA> Handle(ObterQuestaoTurmasProgramaAtendimentoNAAPAPorIdQuery request, CancellationToken cancellationToken)
        {
            return repositorioEncaminhamentoNaapaQuestao.ObterQuestaoTurmasProgramaPorEncaminhamentoId(request.EncaminhamentoNAAPAId);
        }
    }
}
