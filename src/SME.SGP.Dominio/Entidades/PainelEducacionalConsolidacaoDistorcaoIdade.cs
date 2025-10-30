using System;

namespace SME.SGP.Dominio.Entidades
{
    public class PainelEducacionalConsolidacaoDistorcaoIdade
    {
        public int AnoLetivo { get; set; }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public string Modalidade { get; set; }
        public string Ano { get; set; }
        public int QuantidadeAlunos { get; set; }
        public DateTime CriadoEm { get; private set; } = DateTime.UtcNow;
    }
}