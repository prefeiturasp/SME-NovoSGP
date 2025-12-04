using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestaoRespostaAtendimentoNAAPAPorIdQueryHandler : ConsultasBase, IRequestHandler<ObterQuestaoRespostaAtendimentoNAAPAPorIdQuery, IEnumerable<RespostaQuestaoAtendimentoNAAPADto>>
    {
        public IRepositorioQuestaoAtendimentoNAAPA repositorioQuestaoEncaminhamentoNAAPA { get; }

        public ObterQuestaoRespostaAtendimentoNAAPAPorIdQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioQuestaoAtendimentoNAAPA repositorioQuestaoEncaminhamentoNAAPA) : base(contextoAplicacao)
        {
            this.repositorioQuestaoEncaminhamentoNAAPA = repositorioQuestaoEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioQuestaoEncaminhamentoNAAPA));
        }

        public async Task<IEnumerable<RespostaQuestaoAtendimentoNAAPADto>> Handle(ObterQuestaoRespostaAtendimentoNAAPAPorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioQuestaoEncaminhamentoNAAPA.ObterRespostasEncaminhamento(request.Id);
        }
    }
}
