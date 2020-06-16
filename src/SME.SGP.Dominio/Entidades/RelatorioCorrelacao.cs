using System;

namespace SME.SGP.Dominio
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
        public RelatorioCorrelacaoJasper CorrelacaoJasper { get; private set; }
        public Guid Codigo { get; set; }

        public TipoRelatorio TipoRelatorio { get; set; }

        public Usuario UsuarioSolicitante { get; set; }

        public long UsuarioSolicitanteId { get; set; }

        public void AdicionarCorrelacaoJasper(RelatorioCorrelacaoJasper relatorioCorrelacaoJasper)
        {
            CorrelacaoJasper = relatorioCorrelacaoJasper;
        }
    }
}
