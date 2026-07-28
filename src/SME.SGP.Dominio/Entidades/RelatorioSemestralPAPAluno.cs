using Dapper.Contrib.Extensions;
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
        [Computed]
        public RelatorioSemestralTurmaPAP RelatorioSemestralTurmaPAP { get; set; }
        public string AlunoCodigo { get; set; }

        [Computed]
        public List<RelatorioSemestralPAPAlunoSecao> Secoes { get; set; }

        public bool Migrado { get; set; }
        public bool Excluido { get; set; }
    }
}