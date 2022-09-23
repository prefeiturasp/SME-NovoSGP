using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.Evento.ObterDataPossuiEventoLiberacaoExcepcional
{
    public class ObterQuestoesObrigatoriasPorSecoesIdQueryHandler : IRequestHandler<ObterQuestoesObrigatoriasPorSecoesIdQuery, IEnumerable<QuestaoSecaoAeeDto>>
    {
        private readonly IRepositorioQuestao repositorioQuestao;

        public ObterQuestoesObrigatoriasPorSecoesIdQueryHandler(IRepositorioQuestao repositorioQuestao)
        {
            this.repositorioQuestao = repositorioQuestao ?? throw new System.ArgumentNullException(nameof(repositorioQuestao));
        }

        public async Task<IEnumerable<QuestaoSecaoAeeDto>> Handle(ObterQuestoesObrigatoriasPorSecoesIdQuery request, CancellationToken cancellationToken)
        {
            var questoes = await repositorioQuestao.ObterQuestoesPorSecoesId(request.SecoesId, true);

            return questoes;
        }
    }
}
