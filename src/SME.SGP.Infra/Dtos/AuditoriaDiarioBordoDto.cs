using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class AuditoriaDiarioBordoDto : AuditoriaDto
    {
        public long AulaId { get; set; }

        public AuditoriaDiarioBordoDto(AuditoriaDto auditoria, long aulaId)
        {
            Id = auditoria.Id;
            CriadoEm = auditoria.CriadoEm;
            CriadoPor = auditoria.CriadoPor;
            CriadoRF = auditoria.CriadoRF;
            AlteradoEm = auditoria.AlteradoEm;
            AlteradoPor = auditoria.AlteradoPor;
            AlteradoRF = auditoria.AlteradoRF;
            AulaId = aulaId;
        }
    }
}
