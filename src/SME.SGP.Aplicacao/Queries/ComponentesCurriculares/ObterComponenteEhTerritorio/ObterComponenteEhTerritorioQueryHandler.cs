using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponenteEhTerritorioQueryHandler : IRequestHandler<ObterComponenteEhTerritorioQuery, bool>
    {
        private readonly IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular;

        public ObterComponenteEhTerritorioQueryHandler(IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular)
        {
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new System.ArgumentNullException(nameof(repositorioComponenteCurricular));
        }
        public Task<bool> Handle(ObterComponenteEhTerritorioQuery request, CancellationToken cancellationToken)
         => repositorioComponenteCurricular.VerificaComponenteEhRegencia(request.DisciplinaId);
    }
}
