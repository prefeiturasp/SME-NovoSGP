using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesPorIdsQuery : IRequest<IEnumerable<DisciplinaDto>>
    {
        public ObterComponentesCurricularesPorIdsQuery()
        { }

        public ObterComponentesCurricularesPorIdsQuery(long[] ids, bool? possuiTerritorio = false, string codigoTurma = null)
        {
            Ids = ids;
            PossuiTerritorio = possuiTerritorio;
            CodigoTurma = codigoTurma;
        }

        public long[] Ids { get; set; }

        public bool? PossuiTerritorio { get; set; }
        public string CodigoTurma { get; set; }
    }

}
