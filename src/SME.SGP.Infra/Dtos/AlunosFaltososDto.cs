using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class AlunosFaltososDto
    {
        public string TurmaCodigo { get; set; }
        public DateTime DataAula { get; set; }
        public string CodigoAluno { get; set; }
        public int QuantidadeAulas { get; set; }
        public int QuantidadeFaltas { get; set; }
        public Modalidade ModalidadeCodigo { get; set; }
        public string Ano { get; set; }
    }
}
