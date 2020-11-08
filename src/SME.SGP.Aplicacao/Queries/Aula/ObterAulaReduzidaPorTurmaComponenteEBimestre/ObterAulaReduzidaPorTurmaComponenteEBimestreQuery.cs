using MediatR;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAulaReduzidaPorTurmaComponenteEBimestreQuery : IRequest<IEnumerable<AulaReduzidaDto>>
    {
        public ObterAulaReduzidaPorTurmaComponenteEBimestreQuery(long turmaId, bool professorCJ, long componenteCurricularId, long tipoCalendarioId, int bimestre)
        {
            TurmaId = turmaId;
            ProfessorCJ = professorCJ;
            ComponenteCurricularId = componenteCurricularId;
            TipoCalendarioId = tipoCalendarioId;
            Bimestre = bimestre;
        }

        public long TurmaId { get; set; }
        public bool ProfessorCJ { get; set; }
        public long ComponenteCurricularId { get; set; }
        public long TipoCalendarioId { get; set; }
        public int Bimestre { get; set; }
    }
}
