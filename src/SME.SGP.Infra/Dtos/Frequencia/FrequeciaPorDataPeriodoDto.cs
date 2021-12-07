using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class FrequeciaPorDataPeriodoDto
    {
        public FrequeciaPorDataPeriodoDto()
        {
            Aulas = new List<FrequenciaAulaPorDataPeriodoDto>();
        }
        public string CodigoAluno { get; set; }
        public string NomeAluno { get; set; }
        public int NumeroAlunoChamada { get; set; }

        public List<FrequenciaAulaPorDataPeriodoDto> Aulas { get; set; }
    }
}
