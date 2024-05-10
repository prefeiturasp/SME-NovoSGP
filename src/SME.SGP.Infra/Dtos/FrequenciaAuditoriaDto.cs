using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class FrequenciaAuditoriaDto
    {
        public FrequenciaAuditoriaDto()
        {
            AulasIDsComErros = new List<long>();
        }

        public List<long> AulasIDsComErros { get; set; }
        public AuditoriaDto Auditoria { get; set; }

        public void TratarRetornoAuditoria(FrequenciaAuditoriaAulaDto frequenciaAuditoriaAulaDto)
        {
            if (frequenciaAuditoriaAulaDto.Auditoria.NaoEhNulo())
                Auditoria = frequenciaAuditoriaAulaDto.Auditoria;
            
            if (frequenciaAuditoriaAulaDto.AulaIdComErro.HasValue)
                AulasIDsComErros.Add(frequenciaAuditoriaAulaDto.AulaIdComErro.Value);
        }
    }
}
