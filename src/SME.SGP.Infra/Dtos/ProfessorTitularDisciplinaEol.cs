using Newtonsoft.Json;

namespace SME.SGP.Infra
{
    public class ProfessorTitularDisciplinaEol
    {
        [JsonProperty("disciplina_Id")]
        public long DisciplinaId { get; set; }

        [JsonProperty("disciplina")]
        public string DisciplinaNome { get; set; }

        [JsonProperty("nome_Professor")]
        public string ProfessorNome { get; set; }

        [JsonProperty("professorRf")]
        public string ProfessorRf { get; set; }
    }
}