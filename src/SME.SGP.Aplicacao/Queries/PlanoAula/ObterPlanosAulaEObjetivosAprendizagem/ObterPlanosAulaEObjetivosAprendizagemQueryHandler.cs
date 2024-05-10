using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanosAulaEObjetivosAprendizagemQueryHandler : IRequestHandler<ObterPlanosAulaEObjetivosAprendizagemQuery, IEnumerable<PlanoAulaObjetivosAprendizagemDto>>
    {
        private readonly IRepositorioPlanoAula repositorioPlanoAula;
        public ObterPlanosAulaEObjetivosAprendizagemQueryHandler(IRepositorioPlanoAula repositorioPlanoAula)
        {
            this.repositorioPlanoAula = repositorioPlanoAula ?? throw new ArgumentNullException(nameof(repositorioPlanoAula));
        }

        public async Task<IEnumerable<PlanoAulaObjetivosAprendizagemDto>> Handle(ObterPlanosAulaEObjetivosAprendizagemQuery request, CancellationToken cancellationToken)
            => await repositorioPlanoAula.ObterPlanosAulaEObjetivosAprendizagem(request.AulasId);
    }
}
