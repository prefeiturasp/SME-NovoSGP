using System;

namespace SME.SGP.Dominio
{
    public class InformeCorrelacao : EntidadeBase
    {
        public InformeCorrelacao(long informeId, long usuarioSolicitanteId)
        {
            Codigo = Guid.NewGuid();
            InformeId = informeId;
            UsuarioSolicitanteId = usuarioSolicitanteId;
        }

        public InformeCorrelacao()
        {

        }
        public long InformeId { get; set; }
        public Guid Codigo { get; set; }
        public long UsuarioSolicitanteId { get; set; }
    }
}
