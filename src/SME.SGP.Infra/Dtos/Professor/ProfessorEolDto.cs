using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class ProfessorEolDto
    {
        public string CodigoRF { get; set; }
        public string Nome { get; set; }
        public string Cargo { get; set; }
        public string CPF { get; set; }

        public DateTime DataInicioExercicio { get; set; }
    }
}
