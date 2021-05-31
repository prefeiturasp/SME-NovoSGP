using System;

namespace SME.SGP.Infra
{
    public class FiltroCalculoFrequenciaDataRereferenciaDto
    {
        public FiltroCalculoFrequenciaDataRereferenciaDto(DateTime dataReferencia)
        {
            DataReferencia = dataReferencia;
        }

        public DateTime DataReferencia { get; set; }
    }
}
