using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesSgpQueryHandler : IRequestHandler<ObterComponentesCurricularesSgpQuery, IEnumerable<ComponenteCurricularSgp>>
    {
        private readonly IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular;
        public ObterComponentesCurricularesSgpQueryHandler(IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular)
        {
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new System.ArgumentNullException(nameof(repositorioComponenteCurricular));
        }

        public async Task<IEnumerable<ComponenteCurricularSgp>> Handle(ObterComponentesCurricularesSgpQuery request, CancellationToken cancellationToken)
        {
            return await repositorioComponenteCurricular.ListarComponentesCurricularesSgp();
        }
    }
}
