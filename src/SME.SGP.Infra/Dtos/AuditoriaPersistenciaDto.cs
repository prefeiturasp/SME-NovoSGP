using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class AuditoriaPersistenciaDto: AuditoriaDto
    {
        public AuditoriaPersistenciaDto()
        {
            Sucesso = true;
            Mensagens = new List<string>();
        }

        public bool Sucesso { get; set; }
        public string MensagemConsistencia { get; set; }
        public List<string> Mensagens { get; set; }
        public bool EmAprovacao { get; set; }

        public static explicit operator AuditoriaPersistenciaDto(EntidadeBase entidade)
            => new()
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
