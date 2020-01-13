namespace SME.SGP.Infra
{
    public class AtribuicaoCJTitularesRetornoItemDto
    {
        public string Disciplina { get; set; }
        public long DisciplinaId { get; set; }
        public string ProfessorTitular { get; set; }
        public string ProfessorTitularRf { get; set; }
        public bool Substituir { get; set; }
    }
}