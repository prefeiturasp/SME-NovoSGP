using System;

namespace SME.SGP.Dominio.Entidades
{
    public class RelatorioCorrelacao : EntidadeBase
    {
        public RelatorioCorrelacao(TipoRelatorio tipoRelatorio, long usuarioSolicitanteId)
        {
            Codigo = Guid.NewGuid();
            TipoRelatorio = tipoRelatorio;
            UsuarioSolicitanteId = usuarioSolicitanteId;
        }

        public RelatorioCorrelacao()
        {

        }

        public Guid Codigo { get; set; }

        public TipoRelatorio TipoRelatorio { get; set; }

        public Usuario UsuarioSolicitante { get; set; }

        public long UsuarioSolicitanteId { get; set; }
    }
}
