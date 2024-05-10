using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ObjetivosAprendizagem
{
    public class ObterJuremaIdsPorComponentesCurricularIdQueryHandlerFake : IRequestHandler<ObterJuremaIdsPorComponentesCurricularIdQuery, long[]>
    {
        private const long ID_COMPONENTE_CURRICULAR_ARTE = 139;
        private const long ID_JUREMA_COMPONENTE_CURRICULAR_ARTE = 1;

        public async Task<long[]> Handle(ObterJuremaIdsPorComponentesCurricularIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id.Equals(ID_COMPONENTE_CURRICULAR_ARTE))
                return new long[] { ID_JUREMA_COMPONENTE_CURRICULAR_ARTE };
            return new long[] {};
        }
    }
}
