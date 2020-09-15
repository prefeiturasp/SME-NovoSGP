using System.Text.Json.Serialization;

namespace SME.SGP.Infra
{
    public class ProfessorTitularDisciplinaEol
    {
        [JsonPropertyName("disciplina_Id")]
        public long DisciplinaId { get; set; }

        [JsonPropertyName("disciplina")]
        public string DisciplinaNome { get; set; }

        [JsonPropertyName("nome_Professor")]
        public string ProfessorNome { get; set; }

        [JsonPropertyName("professorRf")]
        public string ProfessorRf { get; set; }
    }
}