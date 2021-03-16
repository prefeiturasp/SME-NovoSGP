namespace SME.SGP.Infra
{
    public class FrequenciaAlunoBimestreDto
    {
        public string Bimestre { get; set; }
        public int AulasPrevistas { get; set; }
        public int AulasRealizadas { get; set; }
        public int Ausencias { get; set; }
        public int Compensacoes { get; set; }
        public double Frequencia { get; set; }
        public bool PossuiJustificativas { get; set; }
    }
}