using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.MapeamentoEstudantes;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestaoRespostaMapeamentoEstudantePorIdQueryHandler : ConsultasBase, IRequestHandler<ObterQuestaoRespostaMapeamentoEstudantePorIdQuery, IEnumerable<RespostaQuestaoMapeamentoEstudanteDto>>
    {
        public IRepositorioQuestaoMapeamentoEstudante repositorioQuestao { get; }

        public ObterQuestaoRespostaMapeamentoEstudantePorIdQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioQuestaoMapeamentoEstudante repositorioQuestao) : base(contextoAplicacao)
        {
            this.repositorioQuestao = repositorioQuestao ?? throw new ArgumentNullException(nameof(repositorioQuestao));
        }

        public async Task<IEnumerable<RespostaQuestaoMapeamentoEstudanteDto>> Handle(ObterQuestaoRespostaMapeamentoEstudantePorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioQuestao.ObterRespostasMapeamentoEstudante(request.Id);
        }
    }
}
