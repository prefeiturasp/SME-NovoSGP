using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class PendenciaDiarioBordo : EntidadeBase
    {
        public PendenciaDiarioBordo() { }

        public long PendenciaId { get; set; }
        public Pendencia Pendencia { get; set; }
        public long AulaId { get; set; }
        public Aula Aula{ get; set; }
        public long ComponenteId { get; set; }
        public ComponenteCurricular ComponenteCurricular { get; set; }
        public string ProfessorRf { get; set; }
    }
}
