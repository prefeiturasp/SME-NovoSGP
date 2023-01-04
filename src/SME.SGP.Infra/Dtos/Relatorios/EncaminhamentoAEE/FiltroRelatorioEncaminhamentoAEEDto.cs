namespace SME.SGP.Infra
{
    public class FiltroRelatorioEncaminhamentoAEEDto
    {
        public int AnoLetivo { get; set; }
        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
        public int Modalidade { get; set; }
        public int Semestre { get; set; }
        public string[] CodigosTurma { get; set; }
        public int[] SituacaoIds { get; set; }
        public bool ExibirEncerrados { get; set; }
        public string[] CodigosPAAIResponsavel { get; set; }
        public string UsuarioNome { get; set; }
        public string UsuarioRf { get; set; }
    }
}
