using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class ParametroFrequenciasPersistirDto
    {
        public ParametroFrequenciasPersistirDto() { }
        public ParametroFrequenciasPersistirDto(List<RegistroFrequenciaAluno> frequenciasPersistir)
        {
            FrequenciasPersistir = frequenciasPersistir;
        }

        public List<RegistroFrequenciaAluno> FrequenciasPersistir { get; set; }
    }
}
