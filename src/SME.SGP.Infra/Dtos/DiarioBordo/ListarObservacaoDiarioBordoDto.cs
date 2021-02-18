using System;

namespace SME.SGP.Infra
{
    public class ListarObservacaoDiarioBordoDto
    {

        private DateTime? AlteradoEm { get; set; }
        private string AlteradoPor { get; set; }
        private string AlteradoRF { get; set; }
        private DateTime CriadoEm { get; set; }
        private string CriadoPor { get; set; }
        private string CriadoRF { get; set; }

        public long Id { get; set; }
        public bool Proprietario { get; set; }
        public string Observacao { get; set; }

        public int QtdUsuariosNotificados { get; set; }
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
