using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesQueryHandler : IRequestHandler<ObterComponentesCurricularesQuery, IEnumerable<ComponenteCurricularDto>>
    {
        private readonly IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular;
        public ObterComponentesCurricularesQueryHandler(IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular)
        {
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new System.ArgumentNullException(nameof(repositorioComponenteCurricular));
        }

        public async Task<IEnumerable<ComponenteCurricularDto>> Handle(ObterComponentesCurricularesQuery request, CancellationToken cancellationToken)
        {
            return await repositorioComponenteCurricular.ListarComponentesCurriculares();
        }
    }
}
