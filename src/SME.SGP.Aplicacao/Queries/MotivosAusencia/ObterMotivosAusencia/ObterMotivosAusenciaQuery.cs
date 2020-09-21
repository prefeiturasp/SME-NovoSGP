using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.MotivosAusencia.ObterMotivosAusencia
{
    public class ObterMotivosAusenciaQuery : IRequest<IEnumerable<MotivoAusencia>>
    {
    }
}
