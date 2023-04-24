using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class RespostaTurmaProgramaEncaminhamentoNAAPADto
    {
        public string dreUe { get; set; }
        public string turma { get; set; }
        public string componenteCurricular { get; set; }
        
        public override bool Equals(object o)
        {
            if (!(o is RespostaTurmaProgramaEncaminhamentoNAAPADto)) return false;

            RespostaTurmaProgramaEncaminhamentoNAAPADto resposta = (RespostaTurmaProgramaEncaminhamentoNAAPADto)o;
            return (this.dreUe == resposta.dreUe
                && this.turma == resposta.turma
                && this.componenteCurricular == resposta.componenteCurricular
                );

        }
    }
}
