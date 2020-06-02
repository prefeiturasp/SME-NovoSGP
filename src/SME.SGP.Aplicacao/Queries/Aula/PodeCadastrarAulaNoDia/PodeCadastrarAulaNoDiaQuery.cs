using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class PodeCadastrarAulaNoDiaQuery: IRequest<bool>
    {
        public PodeCadastrarAulaNoDiaQuery(DateTime dataAula, string turmaCodigo, long componenteCurricular)
        {
            DataAula = dataAula;
            TurmaCodigo = turmaCodigo;
            ComponenteCurricular = componenteCurricular;
        }

        public DateTime DataAula { get; set; }
        public string TurmaCodigo { get; set; }
        public long ComponenteCurricular { get; set; }
    }
}
