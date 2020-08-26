using System.Collections.Generic;

namespace SME.SGP.Dominio
{
    public class RelatorioSemestralPAPAluno : EntidadeBase
    {
        public RelatorioSemestralPAPAluno()
        {
            Secoes = new List<RelatorioSemestralPAPAlunoSecao>();
        }

        public long RelatorioSemestralTurmaPAPId { get; set; }
        public RelatorioSemestralTurmaPAP RelatorioSemestralTurmaPAP { get; set; }
        public string AlunoCodigo { get; set; }

        public List<RelatorioSemestralPAPAlunoSecao> Secoes { get; set; }

        public bool Migrado { get; set; }
        public bool Excluido { get; set; }
    }
}