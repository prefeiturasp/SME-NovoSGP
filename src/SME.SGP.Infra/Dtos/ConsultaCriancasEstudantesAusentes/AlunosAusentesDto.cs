using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Infra.Dtos.ConsultaCriancasEstudantesAusentes
{
    public class AlunosAusentesDto
    {
        public int NumeroChamada { get; set; }
        public string Nome { get; set; }
        public string CodigoEol {  get; set; }
        public string FrequenciaGlobal { get; set; }
        public int DiasSeguidosComAusencia { get; set; }
    }
}
