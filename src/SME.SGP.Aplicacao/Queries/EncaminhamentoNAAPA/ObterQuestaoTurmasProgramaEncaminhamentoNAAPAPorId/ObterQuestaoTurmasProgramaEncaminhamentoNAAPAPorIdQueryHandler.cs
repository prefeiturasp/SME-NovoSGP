using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestaoTurmasProgramaEncaminhamentoNAAPAPorIdQueryHandler : ConsultasBase, IRequestHandler<ObterQuestaoTurmasProgramaEncaminhamentoNAAPAPorIdQuery, QuestaoEncaminhamentoNAAPA>
    {
        private readonly IRepositorioQuestaoEncaminhamentoNAAPA repositorioEncaminhamentoNaapaQuestao;
        public ObterQuestaoTurmasProgramaEncaminhamentoNAAPAPorIdQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioQuestaoEncaminhamentoNAAPA repositorioEncaminhamentoNaapaQuestao) : base(contextoAplicacao)
        {
            this.repositorioEncaminhamentoNaapaQuestao = repositorioEncaminhamentoNaapaQuestao ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNaapaQuestao));
        }

        public Task<QuestaoEncaminhamentoNAAPA> Handle(ObterQuestaoTurmasProgramaEncaminhamentoNAAPAPorIdQuery request, CancellationToken cancellationToken)
        {
            return repositorioEncaminhamentoNaapaQuestao.ObterQuestaoTurmasProgramaPorEncaminhamentoId(request.EncaminhamentoNAAPAId);
        }
    }
}
