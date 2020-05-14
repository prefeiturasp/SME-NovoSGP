using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class CompensacaoAusenciaCompletoDto
    {
        public long Id { get; set; }

        public string TurmaId { get; set; }

        public int Bimestre { get; set; }

        public string DisciplinaId { get; set; }

        public string Atividade { get; set; }

        public string Descricao { get; set; }

        public List<DisciplinaNomeDto> DisciplinasRegencia { get; set; }

        public List<CompensacaoAusenciaAlunoCompletoDto> Alunos { get; set; }


        public string CriadoPor { get; set; }
        public string CriadoRf { get; set; }
        public DateTime CriadoEm { get; set; }
        public string AlteradoPor { get; set; }
        public string AlteradoRf { get; set; }
        public DateTime? AlteradoEm { get; set; }
        public bool Migrado { get; set; }
    }
}
