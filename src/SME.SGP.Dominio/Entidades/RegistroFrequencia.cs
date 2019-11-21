using System;

namespace SME.SGP.Dominio
{
    public class RegistroFrequencia : EntidadeBase
    {
        public RegistroFrequencia(Aula aula)
        {
            if (aula.DataAula.Date > DateTime.Now.Date)
                throw new NegocioException("Não é possível registrar frequência em datas futuras.");
            AulaId = aula.Id;
        }

        protected RegistroFrequencia()
        {
        }

        public Aula Aula { get; set; }
        public long AulaId { get; set; }
        public bool Migrado { get; set; }

        public void AdicionarAula(Aula aula)
        {
            Aula = aula;
            AulaId = aula.Id;
        }
    }
}