using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class FechamentoNotaConceitoTurmaDto
    {
        public IList<AlunosFechamentoNotaConceitoTurmaDto> Alunos { get; set; }
        public long FechamentoId { get; set; }
        public DateTime DataFechamento { get; set; }
        public double MediaAprovacaoBimestre { get; set; }
        public TipoNota NotaTipo { get; set; }
        public string AuditoriaAlteracao { get; set; }
        public string AuditoriaInclusao { get; set; }
        public bool PeriodoAberto { get; set; }
        public bool EhRegencia { get; set; }
    }
}
