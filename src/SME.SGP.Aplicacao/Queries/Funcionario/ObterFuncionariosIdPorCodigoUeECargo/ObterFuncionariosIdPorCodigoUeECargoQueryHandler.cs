using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFuncionariosIdPorCodigoUeECargoQueryHandler : IRequestHandler<ObterFuncionariosIdPorCodigoUeECargoQuery, IEnumerable<long>>
    {
        private readonly IServicoNotificacao servicoNotificacao;
        private readonly IMediator mediator;

        public ObterFuncionariosIdPorCodigoUeECargoQueryHandler(IServicoNotificacao servicoNotificacao, IMediator mediator)
        {
            this.servicoNotificacao = servicoNotificacao ?? throw new ArgumentNullException(nameof(servicoNotificacao));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<long>> Handle(ObterFuncionariosIdPorCodigoUeECargoQuery request, CancellationToken cancellationToken)
        {
            var codigosRf = servicoNotificacao.ObterFuncionariosPorNivel(request.CodigoUe, request.Cargo).Select(x => x.Id).ToList();

            return await mediator.Send(new ObterUsuariosIdPorCodigosRfQuery(codigosRf));

        }
    }
}
