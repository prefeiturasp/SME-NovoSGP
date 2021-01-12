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
    public class ObterEncaminhamentoAEEPorIdQueryHandler : ConsultasBase, IRequestHandler<ObterEncaminhamentoAEEPorIdQuery, EncaminhamentoAEE>
    {
        public IMediator mediator { get; }
        public IRepositorioEncaminhamentoAEE repositorioEncaminhamentoAEE { get; }

        public ObterEncaminhamentoAEEPorIdQueryHandler(IContextoAplicacao contextoAplicacao, IMediator mediator, IRepositorioEncaminhamentoAEE repositorioEncaminhamentoAEE) : base(contextoAplicacao)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioEncaminhamentoAEE = repositorioEncaminhamentoAEE ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoAEE));
        }

        public async Task<EncaminhamentoAEE> Handle(ObterEncaminhamentoAEEPorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioEncaminhamentoAEE.ObterEncaminhamentoPorId(request.Id);
        }
    }
}
