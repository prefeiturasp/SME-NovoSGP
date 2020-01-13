using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class CompensacaoAusenciaListagemDto
    {
        public int Bimestre { get; set; }
        public string AtividadeNome { get; set; }
        public List<string> Alunos { get; set; }
    }
}
