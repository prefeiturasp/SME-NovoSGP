namespace SME.SGP.Infra
{
    public class FiltroRelatorioEncaminhamentoNAAPADto
    {
        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
        public int[] SituacaoIds { get; set; }
        public bool ExibirEncerrados { get; set; }
        public int[] FluxoAlertaIds { get; set; }
        public int[] PortaEntradaIds { get; set; }
        public string UsuarioNome { get; set; }
        public string UsuarioRf { get; set; }
        public long[] Ids { get; set; }
    }
}
