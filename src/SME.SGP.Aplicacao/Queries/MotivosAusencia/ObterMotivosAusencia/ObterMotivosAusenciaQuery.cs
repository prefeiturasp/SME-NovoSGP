using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.MotivosAusencia.ObterMotivosAusencia
{
    public class ObterMotivosAusenciaQuery : IRequest<IEnumerable<MotivoAusencia>>
    {
        private static ObterMotivosAusenciaQuery _instance;
        public static ObterMotivosAusenciaQuery Instance => _instance ??= new();
    }
}
