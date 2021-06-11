using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioAbrangenciaAnosLetivosQueryHandler : IRequestHandler<ObterUsuarioAbrangenciaAnosLetivosQuery, IEnumerable<int>>
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;

        public ObterUsuarioAbrangenciaAnosLetivosQueryHandler(IRepositorioAbrangencia repositorioAbrangencia)
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
        }
        public async Task<IEnumerable<int>> Handle(ObterUsuarioAbrangenciaAnosLetivosQuery request, CancellationToken cancellationToken)
        {
            return await repositorioAbrangencia.ObterAnosLetivos(request.Login, request.Perfil, request.ConsideraHistorico, request.AnoMinimo);
        }
    }
}
