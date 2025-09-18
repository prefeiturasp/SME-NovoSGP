namespace SME.SGP.Infra.Dtos.ImportarArquivo
{
    public class ArquivoFluenciaLeitoraDto : ArquivoImportacaoBaseDto
    {
        public ArquivoFluenciaLeitoraDto(string codigoEOLTurma, string codigoEOLAluno, int anoLetivo, int fluencia, int tipoAvaliacao)
        {
            CodigoEOLTurma = codigoEOLTurma;
            CodigoEOLAluno = codigoEOLAluno;
            AnoLetivo = anoLetivo;
            Fluencia = fluencia;
            TipoAvaliacao = tipoAvaliacao;
        }
        public string CodigoEOLTurma { get; set; }
        public string CodigoEOLAluno{ get; set; }
        public int AnoLetivo { get; set; }
        public int Fluencia { get; set; }
        public int TipoAvaliacao { get; set; }
    }
}
