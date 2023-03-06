using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUltimaPendenciaPlanoAEEQueryHandler : IRequestHandler<ObterUltimaPendenciaPlanoAEEQuery, Pendencia>
    {
        private readonly IRepositorioPlanoAEEConsulta repositorioPlanoAEEConsulta;

        public ObterUltimaPendenciaPlanoAEEQueryHandler(IRepositorioPlanoAEEConsulta repositorioPlanoAEEConsulta)
        {
            this.repositorioPlanoAEEConsulta = repositorioPlanoAEEConsulta;
        }

        public async Task<Pendencia> Handle(ObterUltimaPendenciaPlanoAEEQuery request, CancellationToken cancellationToken) =>
            await repositorioPlanoAEEConsulta.ObterUltimaPendenciaPlano(request.PlanoAEEId);
            
    }
}
