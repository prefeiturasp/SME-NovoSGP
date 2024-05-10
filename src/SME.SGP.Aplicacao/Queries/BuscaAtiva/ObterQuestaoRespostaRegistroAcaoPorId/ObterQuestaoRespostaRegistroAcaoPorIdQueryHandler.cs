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
    public class ObterQuestaoRespostaRegistroAcaoPorIdQueryHandler : ConsultasBase, IRequestHandler<ObterQuestaoRespostaRegistroAcaoPorIdQuery, IEnumerable<RespostaQuestaoRegistroAcaoBuscaAtivaDto>>
    {
        public IRepositorioQuestaoRegistroAcaoBuscaAtiva repositorioQuestao { get; }

        public ObterQuestaoRespostaRegistroAcaoPorIdQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioQuestaoRegistroAcaoBuscaAtiva repositorioQuestao) : base(contextoAplicacao)
        {
            this.repositorioQuestao = repositorioQuestao ?? throw new ArgumentNullException(nameof(repositorioQuestao));
        }

        public async Task<IEnumerable<RespostaQuestaoRegistroAcaoBuscaAtivaDto>> Handle(ObterQuestaoRespostaRegistroAcaoPorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioQuestao.ObterRespostasRegistroAcao(request.Id);
        }
    }
}
