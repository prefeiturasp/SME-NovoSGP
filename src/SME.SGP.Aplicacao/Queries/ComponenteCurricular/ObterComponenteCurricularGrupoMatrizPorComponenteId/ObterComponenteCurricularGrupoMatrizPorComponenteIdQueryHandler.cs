using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponenteCurricularGrupoMatrizPorComponenteIdQueryHandler : IRequestHandler<ObterComponenteCurricularGrupoMatrizPorComponenteIdQuery, ComponenteGrupoMatrizDto>
    {
        private readonly IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular;

        public ObterComponenteCurricularGrupoMatrizPorComponenteIdQueryHandler(IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular)
        {
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new ArgumentNullException(nameof(repositorioComponenteCurricular));
        }
        public async Task<ComponenteGrupoMatrizDto> Handle(ObterComponenteCurricularGrupoMatrizPorComponenteIdQuery request, CancellationToken cancellationToken)
             => await repositorioComponenteCurricular.ObterComponenteGrupoMatrizPorId(request.ComponenteCurricularId);
    }
}
