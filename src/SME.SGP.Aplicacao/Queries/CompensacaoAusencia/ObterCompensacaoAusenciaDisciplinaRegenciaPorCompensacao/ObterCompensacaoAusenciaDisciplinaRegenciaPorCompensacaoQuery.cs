using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterCompensacaoAusenciaDisciplinaRegenciaPorCompensacaoQuery : IRequest<IEnumerable<CompensacaoAusenciaDisciplinaRegencia>>
    {
        public ObterCompensacaoAusenciaDisciplinaRegenciaPorCompensacaoQuery(long compensacaoAusenciaId)
        {
            CompensacaoAusenciaId = compensacaoAusenciaId;
        }

        public long CompensacaoAusenciaId { get; }
    }
}
