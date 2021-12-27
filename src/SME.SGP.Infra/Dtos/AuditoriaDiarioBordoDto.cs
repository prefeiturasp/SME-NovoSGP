using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class AuditoriaDiarioBordoDto : AuditoriaDto
    {
        public long AulaId { get; set; }

        public static explicit operator AuditoriaDiarioBordoDto(EntidadeBase entidade)
            => new AuditoriaDiarioBordoDto()
            {
                Id = entidade.Id,
                CriadoEm = entidade.CriadoEm,
                CriadoPor = entidade.CriadoPor,
                CriadoRF = entidade.CriadoRF,
                AlteradoEm = entidade.AlteradoEm,
                AlteradoPor = entidade.AlteradoPor,
                AlteradoRF = entidade.AlteradoRF
            };
    }
}
