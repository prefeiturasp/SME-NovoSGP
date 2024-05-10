using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterNotificacoesPorWorkFlowAprovacaoIdQueryHandler : IRequestHandler<ObterNotificacoesPorWorkFlowAprovacaoIdQuery, IEnumerable<Notificacao>>
    {
        private readonly IRepositorioNotificacaoConsulta repositorioNotificacao;

        public ObterNotificacoesPorWorkFlowAprovacaoIdQueryHandler(IRepositorioNotificacaoConsulta repositorioNotificacao)
        {
            this.repositorioNotificacao = repositorioNotificacao ?? throw new ArgumentNullException(nameof(repositorioNotificacao));
        }

        public async Task<IEnumerable<Notificacao>> Handle(ObterNotificacoesPorWorkFlowAprovacaoIdQuery request, CancellationToken cancellationToken)
            => await repositorioNotificacao.ObterPorWorkFlowAprovacaoId(request.WorkFlowAprovacaoId);
    }
}
