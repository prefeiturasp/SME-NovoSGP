using System;
using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class AuditoriaPersistenciaFechamentoNotaConceitoTurmaDto : AuditoriaPersistenciaDto
    {
        public string AuditoriaAlteracao { get; set; }
        public string AuditoriaInclusao { get; set; }
        public SituacaoFechamento Situacao { get; set; }
        public string SituacaoNome { get; set; }
        public DateTime DataFechamento { get; set; }
        
        public static explicit operator AuditoriaPersistenciaFechamentoNotaConceitoTurmaDto(EntidadeBase entidade)
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