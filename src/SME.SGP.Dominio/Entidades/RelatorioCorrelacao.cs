using SME.SGP.Dominio.Enumerados;
using System;

namespace SME.SGP.Dominio.Entidades
{
    public class RelatorioCorrelacao : EntidadeBase
    {
        public RelatorioCorrelacao(TipoRelatorioEnum tipoRelatorio, Usuario usuarioSolicitante)
        {
            Codigo = Guid.NewGuid();
            TipoRelatorio = tipoRelatorio;
            UsuarioSolicitante = usuarioSolicitante;
        }

        protected RelatorioCorrelacao()
        {

        }

        public Guid Codigo { get; private set; }

        public TipoRelatorioEnum TipoRelatorio { get; private set; }

        public Usuario UsuarioSolicitante { get; private set; }

        public long UsuarioSolicitanteId { get; private set; }
    }
}
