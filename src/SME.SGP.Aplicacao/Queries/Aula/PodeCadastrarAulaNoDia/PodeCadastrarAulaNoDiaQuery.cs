using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class PodeCadastrarAulaNoDiaQuery: IRequest<bool>
    {
        public PodeCadastrarAulaNoDiaQuery(DateTime dataAula, string turmaCodigo, long componenteCurricular, string professorRf)
        {
            DataAula = dataAula;
            TurmaCodigo = turmaCodigo;
            ComponenteCurricular = componenteCurricular;
            ProfessorRf = professorRf;
        }

        public DateTime DataAula { get; set; }
        public string TurmaCodigo { get; set; }
        public long ComponenteCurricular { get; set; }
        public string ProfessorRf { get; set; }
    }
}
