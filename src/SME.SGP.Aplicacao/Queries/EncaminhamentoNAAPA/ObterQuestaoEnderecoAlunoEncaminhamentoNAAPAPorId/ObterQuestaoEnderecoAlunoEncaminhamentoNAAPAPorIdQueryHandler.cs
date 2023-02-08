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
    public class ObterQuestaoEnderecoAlunoEncaminhamentoNAAPAPorIdQueryHandler : ConsultasBase, IRequestHandler<ObterQuestaoEnderecoAlunoEncaminhamentoNAAPAPorIdQuery, QuestaoEncaminhamentoNAAPA>
    {
        private readonly IRepositorioQuestaoEncaminhamentoNAAPA repositorioEncaminhamentoNaapaQuestao;
        public ObterQuestaoEnderecoAlunoEncaminhamentoNAAPAPorIdQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioQuestaoEncaminhamentoNAAPA repositorioEncaminhamentoNaapaQuestao) : base(contextoAplicacao)
        {
            this.repositorioEncaminhamentoNaapaQuestao = repositorioEncaminhamentoNaapaQuestao ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNaapaQuestao));
        }

        public Task<QuestaoEncaminhamentoNAAPA> Handle(ObterQuestaoEnderecoAlunoEncaminhamentoNAAPAPorIdQuery request, CancellationToken cancellationToken)
        {
            return repositorioEncaminhamentoNaapaQuestao.ObterQuestaoEnderecoResidencialPorEncaminhamentoId(request.EncaminhamentoNAAPAId);
        }
    }
}
