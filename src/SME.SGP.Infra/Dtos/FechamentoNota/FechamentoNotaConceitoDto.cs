namespace SME.SGP.Infra
{
    public class FechamentoNotaConceitoDto
    {
        public long DiscplinaId { get; set; }
        public string CodigoAluno { get; set; }
        public long? ConceitoId { get; set; }
        public double? Nota { get; set; }
    }
}