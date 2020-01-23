namespace SME.SGP.Infra
{
    public class DisciplinaDto
    {
        public int CodigoComponenteCurricular { get; set; }
        public string Nome { get; set; }
        public bool PossuiObjetivos { get; set; }
        public bool Regencia { get; set; }
        public bool Compartilhada { get; set; }
        public bool RegistroFrequencia { get; set; }
    }
}