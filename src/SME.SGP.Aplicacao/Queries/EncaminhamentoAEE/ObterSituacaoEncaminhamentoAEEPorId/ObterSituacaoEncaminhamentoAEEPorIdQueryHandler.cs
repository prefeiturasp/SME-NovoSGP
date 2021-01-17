using MediatR;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterSituacaoEncaminhamentoAEEPorIdQueryHandler : IRequestHandler<ObterSituacaoEncaminhamentoAEEPorIdQuery, SituacaoAEE>
    {
        public IMediator mediator { get; }
        public IRepositorioEncaminhamentoAEE repositorioEncaminhamentoAEE { get; }

        public ObterSituacaoEncaminhamentoAEEPorIdQueryHandler(IMediator mediator, IRepositorioEncaminhamentoAEE repositorioEncaminhamentoAEE)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioEncaminhamentoAEE = repositorioEncaminhamentoAEE ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoAEE));
        }

        public async Task<SituacaoAEE> Handle(ObterSituacaoEncaminhamentoAEEPorIdQuery request, CancellationToken cancellationToken)
                => await repositorioEncaminhamentoAEE.ObterSituacaoEncaminhamentoAEE(request.EncaminhamentoAeeId);
    }
}
