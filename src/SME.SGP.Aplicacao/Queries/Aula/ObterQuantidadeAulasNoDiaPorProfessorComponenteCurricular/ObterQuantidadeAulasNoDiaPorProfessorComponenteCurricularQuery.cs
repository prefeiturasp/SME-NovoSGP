using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeAulasNoDiaPorProfessorComponenteCurricularQuery: IRequest<int>
    {
        public ObterQuantidadeAulasNoDiaPorProfessorComponenteCurricularQuery(string turmaCodigo, long componenteCurricular, DateTime data, string professorRf, bool experienciaPedagogica)
        {
            TurmaCodigo = turmaCodigo;
            ComponenteCurricular = componenteCurricular;
            Data = data;
            ProfessorRf = professorRf;
            ExperienciaPedagogica = experienciaPedagogica;
        }

        public string TurmaCodigo { get; set; }
        public long ComponenteCurricular { get; set; }
        public DateTime Data { get; set; }
        public string ProfessorRf { get; set; }
        public bool ExperienciaPedagogica { get; set; }
    }
}
