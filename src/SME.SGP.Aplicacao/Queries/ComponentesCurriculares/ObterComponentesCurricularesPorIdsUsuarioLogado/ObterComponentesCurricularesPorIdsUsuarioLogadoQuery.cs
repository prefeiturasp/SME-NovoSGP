using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesPorIdsUsuarioLogadoQuery : IRequest<IEnumerable<DisciplinaDto>>
    {
        public ObterComponentesCurricularesPorIdsUsuarioLogadoQuery(long[] ids, string codigoTurma = null)
        {
            Ids = ids;
            CodigoTurma = codigoTurma;
        }

        public long[] Ids { get; set; }
        public string CodigoTurma { get; set; }
    }
}
