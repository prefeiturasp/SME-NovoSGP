using MediatR;
using SME.SGP.Infra.Dtos.Abrangencia;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.ObterUsuariosPerfis
{
    public class ObterUsuariosPerfisQuery : IRequest<IEnumerable<AbrangenciaUsuarioPerfilDto>>
    {
    }
}
