using SME.SGP.Dominio;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Aplicacao
{
    public static class ConceitoExtensao
    {
        public static Dictionary<long, Conceito> ToDicionarioPorId(this IEnumerable<Conceito> conceitos)
        {
            if (conceitos.EhNulo())
                return new Dictionary<long, Conceito>();

            return conceitos.ToDictionary(conceito => conceito.Id);
        }

        public static Conceito ObterConceitoOuNulo(this IDictionary<long, Conceito> conceitosPorId, long id)
        {
            if (conceitosPorId.EhNulo())
                return null;

            return conceitosPorId.TryGetValue(id, out var conceito) ? conceito : null;
        }
    }
}
