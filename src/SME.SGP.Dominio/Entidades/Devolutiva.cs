using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class Devolutiva: EntidadeBase
    {
        public Devolutiva()
        {
            DiariosBordo = new List<DiarioBordo>();
        }

        public string Descricao { get; set; }

        public List<DiarioBordo> DiariosBordo { get; set; }

        public long CodigoComponenteCurricular { get; set; }

        public DateTime PeriodoInicio { get; set; }

        public DateTime PeriodoFim { get; set; }

        public bool Excluido { get; set; }
    }
}
