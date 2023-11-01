using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterIdsNotificacaoPorInformativoIdQueryHandler : IRequestHandler<ObterIdsNotificacaoPorInformativoIdQuery, IEnumerable<long>>
    {
        private readonly IRepositorioInformativoNotificacao repositorio;

        public ObterIdsNotificacaoPorInformativoIdQueryHandler(IRepositorioInformativoNotificacao repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public Task<IEnumerable<long>> Handle(ObterIdsNotificacaoPorInformativoIdQuery request, CancellationToken cancellationToken)
        {
            return repositorio.ObterIdsNotificacoesPorInformativoIdAsync(request.InformativoId);
        }
    }
}
