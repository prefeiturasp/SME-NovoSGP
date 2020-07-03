using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterGradeAulasPorTurmaEProfessorQuery: IRequest<GradeComponenteTurmaAulasDto>
    {
        public ObterGradeAulasPorTurmaEProfessorQuery(string turmaCodigo, long componenteCurricular, DateTime dataAula, string codigoRf = null, bool ehRegencia = false)
        {
            TurmaCodigo = turmaCodigo;
            ComponenteCurricular = componenteCurricular;
            DataAula = dataAula;
            CodigoRf = codigoRf;
            EhRegencia = ehRegencia;
        }

        public string TurmaCodigo { get; set; }
        public long ComponenteCurricular { get; set; }
        public DateTime DataAula { get; set; }
        public string CodigoRf { get; set; }
        public bool EhRegencia { get; set; }
    }
}
