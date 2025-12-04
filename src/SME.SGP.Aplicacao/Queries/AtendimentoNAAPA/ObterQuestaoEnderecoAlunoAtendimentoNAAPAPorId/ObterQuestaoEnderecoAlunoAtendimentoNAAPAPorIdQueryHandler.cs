using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestaoEnderecoAlunoAtendimentoNAAPAPorIdQueryHandler : ConsultasBase, IRequestHandler<ObterQuestaoEnderecoAlunoAtendimentoNAAPAPorIdQuery, QuestaoEncaminhamentoNAAPA>
    {
        private readonly IRepositorioQuestaoEncaminhamentoNAAPA repositorioEncaminhamentoNaapaQuestao;
        public ObterQuestaoEnderecoAlunoAtendimentoNAAPAPorIdQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioQuestaoEncaminhamentoNAAPA repositorioEncaminhamentoNaapaQuestao) : base(contextoAplicacao)
        {
            this.repositorioEncaminhamentoNaapaQuestao = repositorioEncaminhamentoNaapaQuestao ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNaapaQuestao));
        }

        public Task<QuestaoEncaminhamentoNAAPA> Handle(ObterQuestaoEnderecoAlunoAtendimentoNAAPAPorIdQuery request, CancellationToken cancellationToken)
        {
            return repositorioEncaminhamentoNaapaQuestao.ObterQuestaoEnderecoResidencialPorEncaminhamentoId(request.EncaminhamentoNAAPAId);
        }
    }
}
