namespace SME.SGP.Dto
{
    public class DisciplinaDto
    {
        public DisciplinaDto()
        {
            Regencia = false;
        }

        public int CodigoComponenteCurricular { get; set; }
        public string Nome { get; set; }
        public bool PossuiObjetivos { get; set; }
        public bool Regencia { get; set; }
    }
}