using System;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class RegistroFrequenciaPorDataPeriodoDto
    {
        public RegistroFrequenciaPorDataPeriodoDto()
        {
            ListaFrequencia = new List<FrequeciaPorDataPeriodoDto>();
        }
        public DateTime? AlteradoEm { get; set; }

        public string AlteradoPor { get; set; }

        public string AlteradoRF { get; set; }

        public DateTime? CriadoEm { get; set; }

        public string CriadoPor { get; set; }

        public string CriadoRF { get; set; }

        [ListaTemElementos(ErrorMessage = "A lista de frequência é obrigatória")]
        public IList<FrequeciaPorDataPeriodoDto> ListaFrequencia { get; set; }
    }
}
