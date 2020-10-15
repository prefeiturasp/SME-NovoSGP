using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanoAulaEObjetivosAprendizagemQueryHandler : IRequestHandler<ObterPlanoAulaEObjetivosAprendizagemQuery, PlanoAulaObjetivosAprendizagemDto>
    {
        private readonly IRepositorioPlanoAula repositorioPlanoAula;
        public ObterPlanoAulaEObjetivosAprendizagemQueryHandler(IRepositorioPlanoAula repositorioPlanoAula)
        {
            this.repositorioPlanoAula = repositorioPlanoAula ?? throw new ArgumentNullException(nameof(repositorioPlanoAula));
        }

        public async Task<PlanoAulaObjetivosAprendizagemDto> Handle(ObterPlanoAulaEObjetivosAprendizagemQuery request, CancellationToken cancellationToken)
            => await repositorioPlanoAula.ObterPlanoAulaEObjetivosAprendizagem(request.AulaId);
    }
}
