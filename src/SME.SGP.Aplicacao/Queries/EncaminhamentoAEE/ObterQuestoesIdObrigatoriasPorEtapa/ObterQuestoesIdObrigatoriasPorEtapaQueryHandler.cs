using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterQuestoesIdObrigatoriasPorEtapaQueryHandler : IRequestHandler<ObterQuestoesIdObrigatoriasPorEtapaQuery, IEnumerable<QuestaoIdSecaoAeeDto>>
    {
        private readonly IRepositorioQuestao repositorioQuestao;

        public ObterQuestoesIdObrigatoriasPorEtapaQueryHandler(IRepositorioQuestao repositorioQuestao)
        {
            this.repositorioQuestao = repositorioQuestao ?? throw new System.ArgumentNullException(nameof(repositorioQuestao));
        }

        public async Task<IEnumerable<QuestaoIdSecaoAeeDto>> Handle(ObterQuestoesIdObrigatoriasPorEtapaQuery request, CancellationToken cancellationToken)
        {
            var questoes = await repositorioQuestao.ObterQuestoesIdPorEtapa(request.Etapa, true);

            return questoes;
        }
    }
}
