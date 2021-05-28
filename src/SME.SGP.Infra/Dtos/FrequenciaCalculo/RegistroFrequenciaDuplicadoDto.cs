namespace SME.SGP.Infra
{
    public class RegistroFrequenciaDuplicadoDto
    {
        public string TurmaCodigo { get; set; }
        public string AlunoCodigo { get; set; }
        public string DisciplinaId { get; set; }
        public long PeriodoEscolarId { get; set; }
        public long UltimoId { get; set; }
    }
}
