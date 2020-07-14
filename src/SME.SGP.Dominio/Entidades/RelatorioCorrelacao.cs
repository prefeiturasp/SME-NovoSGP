using System;

namespace SME.SGP.Dominio
{
    public class RelatorioCorrelacao : EntidadeBase, ICloneable
    {
        public RelatorioCorrelacao(TipoRelatorio tipoRelatorio, long usuarioSolicitanteId, TipoFormatoRelatorio formato)
        {
            Codigo = Guid.NewGuid();
            TipoRelatorio = tipoRelatorio;
            UsuarioSolicitanteId = usuarioSolicitanteId;
            Formato = formato;
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

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public bool EhRelatorioJasper => TipoRelatorio.EhUmDosValores(TipoRelatorio.Boletim, TipoRelatorio.ConselhoClasseAluno, TipoRelatorio.ConselhoClasseTurma);

        public bool PrazoDownloadExpirado => (DateTime.Now - CriadoEm).Days > 1;
        public TipoFormatoRelatorio  Formato { get; set; }
    }
}
