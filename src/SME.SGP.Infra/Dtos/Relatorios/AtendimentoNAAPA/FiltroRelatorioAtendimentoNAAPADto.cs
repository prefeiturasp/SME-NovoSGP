using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class FiltroRelatorioAtendimentoNAAPADto
    {
        public bool ConsideraHistorico { get; set; }
        public int AnoLetivo { get; set; }
        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
        public Modalidade[] Modalidades { get; set; }
        public string[] AnosEscolaresCodigos { get; set; }
        public int[] SituacaoIds { get; set; }
        public bool ExibirEncerrados { get; set; }
        public int[] FluxoAlertaIds { get; set; }
        public int[] PortaEntradaIds { get; set; }
        public string UsuarioNome { get; set; }
        public string UsuarioRf { get; set; }
    }
}
