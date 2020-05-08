using System.Collections.Generic;

namespace SME.SGP.Dominio
{
    public class RelatorioSemestralAluno : EntidadeBase
    {
        public RelatorioSemestralAluno()
        {
            Secoes = new List<RelatorioSemestralAlunoSecao>();
        }

        public long RelatorioSemestralId { get; set; }
        public RelatorioSemestral RelatorioSemestral { get; set; }
        public string AlunoCodigo { get; set; }

        public List<RelatorioSemestralAlunoSecao> Secoes { get; set; }

        public bool Migrado { get; set; }
        public bool Excluido { get; set; }
    }
}