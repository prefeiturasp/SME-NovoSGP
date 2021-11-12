namespace SME.SGP.Infra
{
    public class AlunoFrequenciaDto
    {
        public long AlunoRf { get; set; }
        public string Nome { get; set; }
        public int NumeroChamada { get; set; }
        public int Ausencias { get; set; }
        public int Compensacoes { get; set; }
        public string Frequencia { get; set; }
        public int Remotos { get; set; }
        public int Presencas { get; set; }
        public bool PossuiJustificativas { get; set; }
        public MarcadorFrequenciaDto MarcadorFrequencia { get; set; }
        public bool EhAtendidoAEE { get; set; }
    }
}