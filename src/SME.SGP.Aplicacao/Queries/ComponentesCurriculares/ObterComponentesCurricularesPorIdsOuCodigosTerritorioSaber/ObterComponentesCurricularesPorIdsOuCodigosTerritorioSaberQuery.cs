using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesPorIdsOuCodigosTerritorioSaberQuery : IRequest<IEnumerable<DisciplinaDto>>
    {
        public ObterComponentesCurricularesPorIdsOuCodigosTerritorioSaberQuery(IList<(long codigo, long? codigoTerritorioSaber)> codigosComponentes, bool? possuiTerritorio = false, string codigoTurma = null)
        {
            CodigoComponentes = codigosComponentes;
            PossuiTerritorio = possuiTerritorio;
            CodigoTurma = codigoTurma;
        }

        public IList<(long codigo, long? codigoTerritorioSaber)> CodigoComponentes { get; set; }
        public bool? PossuiTerritorio { get; set; }
        public string CodigoTurma { get; set; }
    }
}
