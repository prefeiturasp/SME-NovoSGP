using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class CompensacaoAusenciaListagemDto
    {
        public long Id { get; set; }
        public int Bimestre { get; set; }
        public string AtividadeNome { get; set; }
        public List<string> Alunos { get; set; }
    }
}
