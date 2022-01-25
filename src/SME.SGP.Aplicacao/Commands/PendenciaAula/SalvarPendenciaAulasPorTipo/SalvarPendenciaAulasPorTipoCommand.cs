using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class SalvarPendenciaAulasPorTipoCommand : IRequest
    {
        public SalvarPendenciaAulasPorTipoCommand(IEnumerable<Aula> aulas, TipoPendencia tipoPendenciaAula)
        {
            Aulas = aulas;
            TipoPendenciaAula = tipoPendenciaAula;
        }

        public IEnumerable<Aula> Aulas { get; }
        public TipoPendencia TipoPendenciaAula { get; }
    }
}
