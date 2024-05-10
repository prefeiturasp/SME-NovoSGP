using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterNotificacoesPorAnoLetivoERfQueryHandler : IRequestHandler<ObterNotificacoesPorAnoLetivoERfQuery, IEnumerable<Notificacao>>
    {
        private readonly IRepositorioNotificacaoConsulta repositorioNotificacaoConsulta;

        public ObterNotificacoesPorAnoLetivoERfQueryHandler(IRepositorioNotificacaoConsulta repositorio)
        {
            this.repositorioNotificacaoConsulta = repositorio;
        }

        public async Task<IEnumerable<Notificacao>> Handle(ObterNotificacoesPorAnoLetivoERfQuery request, CancellationToken cancellationToken)
            => await repositorioNotificacaoConsulta.ObterNotificacoesPorAnoLetivoERf(request.AnoLetivo, request.UsuarioRf, request.Limite);
    }
}