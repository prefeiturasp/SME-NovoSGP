using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterObjetivosPlanoDisciplinaQueryHandler : AbstractUseCase, IRequestHandler<ObterObjetivosPlanoDisciplinaQuery, IEnumerable<ObjetivosAprendizagemPorComponenteDto>>
    {

        private readonly IRepositorioObjetivoAprendizagemPlano repositorioObjetivosPlano;
        public ObterObjetivosPlanoDisciplinaQueryHandler(IRepositorioObjetivoAprendizagemPlano repositorioObjetivosPlano,
                                                         IMediator mediator) : base(mediator)
        {
            this.repositorioObjetivosPlano = repositorioObjetivosPlano ?? throw new ArgumentNullException(nameof(repositorioObjetivosPlano));
        }

        public async Task<IEnumerable<ObjetivosAprendizagemPorComponenteDto>> Handle(ObterObjetivosPlanoDisciplinaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioObjetivosPlano.ObterObjetivosPorComponenteNoPlano(request.Bimestre,
                                                                                    request.TurmaId,
                                                                                    request.ComponenteCurricularId,
                                                                                    request.DisciplinaId,
                                                                                    request.FiltrarSomenteRegencia);

        }

    }
}
