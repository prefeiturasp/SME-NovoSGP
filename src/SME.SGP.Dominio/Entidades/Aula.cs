using System;

namespace SME.SGP.Dominio
{
    public class Aula : EntidadeBase
    {
        public TipoAula TipoAula { get; set; }
        public int DisciplinaId { get; set; }
        public int Quantidade { get; set; }
        public DateTime Data { get; set; }
        public RecorrenciaAula RecorrenciaAula { get; set; }
        public bool Excluido { get; set; }
    }
}
