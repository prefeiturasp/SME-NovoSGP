namespace SME.SGP.Dominio
{
    public class AnotacaoFrequenciaAluno : EntidadeBase
    {
        public long MotivoAusenciaId { get; set; }
        public long AulaId { get; set; }
        public string Anotacao { get; set; }
        public string CodigoAluno { get; set; }
        public bool Excluido { get; set; }
    }
}
