using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterObjetivosAprendizagemAulaPorPlanoAulaIdQueryHandler : IRequestHandler<ObterObjetivosAprendizagemAulaPorPlanoAulaIdQuery, IEnumerable<ObjetivoAprendizagemAula>>
    {
        private readonly IRepositorioObjetivoAprendizagemAula repositorioObjetivoAprendizagemAula;

        public ObterObjetivosAprendizagemAulaPorPlanoAulaIdQueryHandler(IRepositorioObjetivoAprendizagemAula repositorioObjetivoAprendizagemAula)
        {
            this.repositorioObjetivoAprendizagemAula = repositorioObjetivoAprendizagemAula ?? throw new ArgumentNullException(nameof(repositorioObjetivoAprendizagemAula));
        }

        public Task<IEnumerable<ObjetivoAprendizagemAula>> Handle(ObterObjetivosAprendizagemAulaPorPlanoAulaIdQuery request, CancellationToken cancellationToken)
            => repositorioObjetivoAprendizagemAula.ObterObjetivosAprendizagemAulaPorPlanoAulaId(request.PlanoAulaId);
    }
}
