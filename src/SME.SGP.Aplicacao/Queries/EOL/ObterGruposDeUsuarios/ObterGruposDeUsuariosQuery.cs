using MediatR;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterGruposDeUsuariosQuery : IRequest<IEnumerable<GruposDeUsuariosDto>>
    {
    }
}
