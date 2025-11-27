using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Infra.Dtos.NovoEncaminhamentoNAAPA
{
    public class NovoEncaminhamentoNAAPAResumoDto
    {
        public long Id { get; set; }
        public int TipoQuestionario { get; set; }
        public string UeNome { get; set; }
        public string NomeAluno { get; set; }
        public string TurmaNome { get; set; }
        public DateTime? DataAberturaQueixaInicio { get; set; }
        public DateTime? DataUltimoAtendimento { get; set; }
        public int Situacao { get; set; }
        public bool SuspeitaViolencia { get; set; }
    }
}