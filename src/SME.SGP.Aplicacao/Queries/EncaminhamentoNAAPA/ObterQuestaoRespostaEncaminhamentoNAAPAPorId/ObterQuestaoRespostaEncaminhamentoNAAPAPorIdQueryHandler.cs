using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestaoRespostaEncaminhamentoNAAPAPorIdQueryHandler : ConsultasBase, IRequestHandler<ObterQuestaoRespostaEncaminhamentoNAAPAPorIdQuery, IEnumerable<RespostaQuestaoAtendimentoNAAPADto>>
    {
        public IRepositorioQuestaoEncaminhamentoNAAPA repositorioQuestaoEncaminhamentoNAAPA { get; }

        public ObterQuestaoRespostaEncaminhamentoNAAPAPorIdQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioQuestaoEncaminhamentoNAAPA repositorioQuestaoEncaminhamentoNAAPA) : base(contextoAplicacao)
        {
            this.repositorioQuestaoEncaminhamentoNAAPA = repositorioQuestaoEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioQuestaoEncaminhamentoNAAPA));
        }

        public async Task<IEnumerable<RespostaQuestaoAtendimentoNAAPADto>> Handle(ObterQuestaoRespostaEncaminhamentoNAAPAPorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioQuestaoEncaminhamentoNAAPA.ObterRespostasEncaminhamento(request.Id);
        }
    }
}
