using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterGradeAulasPorTurmaEProfessorQuery: IRequest<GradeComponenteTurmaAulasDto>
    {
        public ObterGradeAulasPorTurmaEProfessorQuery(string turmaCodigo, long[] componentesCurriculares, DateTime dataAula, string codigoRf = null, bool ehRegencia = false, bool ehGestor = false)
        {
            TurmaCodigo = turmaCodigo;
            ComponentesCurriculares = componentesCurriculares;
            DataAula = dataAula;
            CodigoRf = codigoRf;
            EhRegencia = ehRegencia;
            EhGestor = ehGestor;
        }

        public string TurmaCodigo { get; set; }
        public long[] ComponentesCurriculares { get; set; }
        public DateTime DataAula { get; set; }
        public string CodigoRf { get; set; }
        public bool EhRegencia { get; set; }
        public bool EhGestor { get; set; }
    }
}
