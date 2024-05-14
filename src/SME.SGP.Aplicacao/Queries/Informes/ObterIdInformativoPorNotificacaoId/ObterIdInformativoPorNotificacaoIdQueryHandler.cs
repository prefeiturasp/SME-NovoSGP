using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterIdInformativoPorNotificacaoIdQueryHandler : IRequestHandler<ObterIdInformativoPorNotificacaoIdQuery, long>
    {
        private readonly IRepositorioInformativoNotificacao repositorio;

        public ObterIdInformativoPorNotificacaoIdQueryHandler(IRepositorioInformativoNotificacao repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public Task<long> Handle(ObterIdInformativoPorNotificacaoIdQuery request, CancellationToken cancellationToken)
        {
            return repositorio.ObterIdInformativoPorNotificacaoIdAsync(request.NotificacaoId);
        }
    }
}
