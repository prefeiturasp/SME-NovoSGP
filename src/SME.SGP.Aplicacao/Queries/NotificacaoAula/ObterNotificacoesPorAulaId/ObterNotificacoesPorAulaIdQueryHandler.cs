using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterNotificacoesPorAulaIdQueryHandler : IRequestHandler<ObterNotificacoesPorAulaIdQuery, IEnumerable<NotificacaoAula>>
    {
        private readonly IRepositorioNotificacaoAula repositorioNotificacaoAula;

        public ObterNotificacoesPorAulaIdQueryHandler(IRepositorioNotificacaoAula repositorioNotificacaoAula)
        {
            this.repositorioNotificacaoAula = repositorioNotificacaoAula ?? throw new ArgumentNullException(nameof(repositorioNotificacaoAula));
        }

        public async Task<IEnumerable<NotificacaoAula>> Handle(ObterNotificacoesPorAulaIdQuery request, CancellationToken cancellationToken)
            => await repositorioNotificacaoAula.ObterPorAulaAsync(request.AulaId);
    }
}
