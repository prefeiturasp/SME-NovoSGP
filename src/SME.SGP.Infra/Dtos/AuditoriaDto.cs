using SME.SGP.Dominio;
using System;

namespace SME.SGP.Infra
{
    public class AuditoriaDto
    {
        public long Id { get; set; }
        public DateTime? AlteradoEm { get; set; }
        public string AlteradoPor { get; set; }
        public string AlteradoRF { get; set; }
        public DateTime CriadoEm { get; set; }
        public string CriadoPor { get; set; }
        public string CriadoRF { get; set; }

        public static explicit operator AuditoriaDto(EntidadeBase entidade)
            => entidade == null ? null :
            new AuditoriaDto()
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