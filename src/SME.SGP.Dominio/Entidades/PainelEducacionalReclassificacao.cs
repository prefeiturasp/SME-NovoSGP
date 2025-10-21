using System;

namespace SME.SGP.Dominio.Entidades
{
    public class PainelEducacionalReclassificacao
    {
        public string Dre { get; set; }
        public string Ue { get; set; }
        public int Ano { get; set; }
        public Modalidade ModalidadeTurma { get; set; }
        public int QuantidadeAlunosReclassificados { get; set; }
        public DateTime CriadoEm { get; private set; } = DateTime.UtcNow;
    }
}
