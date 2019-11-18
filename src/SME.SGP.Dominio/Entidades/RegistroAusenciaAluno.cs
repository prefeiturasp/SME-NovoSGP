namespace SME.SGP.Dominio
{
    public class RegistroAusenciaAluno : EntidadeBase
    {
        public string CodigoAluno { get; set; }
        public bool Migrado { get; set; }
        public int NumeroAula { get; set; }
        public RegistroFrequencia RegistroFrequencia { get; set; }
        public long RegistroFrequenciaId { get; set; }
    }
}