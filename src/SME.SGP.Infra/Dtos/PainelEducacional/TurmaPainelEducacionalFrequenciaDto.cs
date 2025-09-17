namespace SME.SGP.Infra.Dtos.PainelEducacional
{
    public class TurmaPainelEducacionalFrequenciaDto
    {
        public long UeId { get; set; }
        public long TurmaId { get; set; }
        public string Nome { get; set; }
        public int ModalidadeCodigo { get; set; }
        public string AnoLetivo { get; set; }
        public string Ano { get; set; }
    }
}
