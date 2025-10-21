using System;

namespace SME.SGP.Dominio.Entidades
{
    public class PainelEducacionalReclassificacao
    {
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public int AnoLetivo { get; set; }
        public string AnoTurma { get; set; }
        public Modalidade ModalidadeTurma { get; set; }
        public int QuantidadeAlunosReclassificados { get; set; }
        public DateTime CriadoEm { get; private set; } = DateTime.UtcNow;
    }
}
