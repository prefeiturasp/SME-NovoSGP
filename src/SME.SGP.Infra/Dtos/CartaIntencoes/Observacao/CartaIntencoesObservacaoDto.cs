using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class CartaIntencoesObservacaoDto
    {
        private DateTime? AlteradoEm { get; set; }
        private string AlteradoPor { get; set; }
        private string AlteradoRF { get; set; }
        private DateTime CriadoEm { get; set; }
        private string CriadoPor { get; set; }
        private string CriadoRF { get; set; }

        public long Id { get; set; }
        public long TurmaId { get; set; }
        public long ComponenteCurricularId { get; set; }
        public bool Proprietario { get; set; }
        public string Observacao { get; set; }
        public AuditoriaDto Auditoria => new AuditoriaDto
        {
            CriadoEm = CriadoEm,
            CriadoPor = CriadoPor,
            CriadoRF = CriadoRF,
            AlteradoEm = AlteradoEm,
            AlteradoPor = AlteradoPor,
            AlteradoRF = AlteradoRF
        };
    }
}
