using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
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