namespace SME.SGP.Infra
{
    public class ConselhoClasseAlunoNotaDto
    {
        public string AlunoCodigo { get; set; }
        public string Nota { get; set; }
        public long ComponenteCurricularId { get; set; }
        public string Descricao { get; set; }
    }
}