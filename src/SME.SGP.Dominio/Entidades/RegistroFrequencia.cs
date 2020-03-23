using System;

namespace SME.SGP.Dominio
{
    public class RegistroFrequencia : EntidadeBase
    {
        public RegistroFrequencia(Aula aula)
        {
            AulaId = aula.Id;
        }

        protected RegistroFrequencia()
        {
        }

        public Aula Aula { get; set; }
        public long AulaId { get; set; }
        public bool Migrado { get; set; }
        public bool Excluido { get; set; }

        public void AdicionarAula(Aula aula)
        {
            Aula = aula;
            AulaId = aula.Id;
        }
    }
}