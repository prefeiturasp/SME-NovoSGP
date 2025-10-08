using System;

namespace SME.SGP.Dominio.Entidades
{
    public class PainelEducacionalAbandono
    {
        public int Id { get; set; }
        public string Dre { get; set; }
        public int Ano { get; set; }
        public string ModalidadeTurma { get; set; }
        public string Turma { get; set; }
        public int QuantidadeAlunoDesistentes { get; set; }
        public DateTime CriadoEm { get; set; }
    }
}
