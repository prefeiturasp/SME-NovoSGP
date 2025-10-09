using System;

namespace SME.SGP.Dominio.Entidades
{
    public class PainelEducacionalAbandono
    {
        public int Id { get; set; }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public int AnoLetivo { get; set; }
        public string Ano { get; set; }
        public string Modalidade { get; set; }
        public string Turma { get; set; }
        public int QuantidadeDesistencias { get; set; }
        public DateTime CriadoEm { get; set; }
    }
}
