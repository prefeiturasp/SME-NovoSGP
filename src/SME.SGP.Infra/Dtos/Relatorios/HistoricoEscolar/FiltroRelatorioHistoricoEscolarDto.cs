namespace SME.SGP.Infra.Dtos.Relatorios.HistoricoEscolar
{
    public class FiltroRelatorioHistoricoEscolarDto
    {
        public string AnoLetivo { get; set; }
        public string Nome { get; set; }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public string CodigoTurma { get; set; }
        public int Semestre { get; set; }
        public string[] AlunosCodigo { get; set; }
        public bool ImprimirDadosResp { get; set; }
        public bool PreencherDataImpressao { get; set; }
    }

}
