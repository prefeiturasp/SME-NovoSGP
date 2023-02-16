using Newtonsoft.Json;
using System.Linq;

namespace SME.SGP.Infra
{
    public class ProfessorTitularDisciplinaEol
    {
        private long[] disciplinasId;

        [JsonProperty("disciplinas_Id")]
        public string CodigosDisciplinas { get; set; }
        public long[] DisciplinasId { get => CodigosDisciplinas?.Split(",").Select(x => long.Parse(x)).ToArray() ?? Enumerable.Empty<long>().ToArray(); set { disciplinasId = value; } }

        [JsonProperty("disciplina")]
        public string DisciplinaNome { get; set; }

        [JsonProperty("nome_Professor")]
        public string ProfessorNome { get; set; }

        [JsonProperty("professorRf")]
        public string ProfessorRf { get; set; }

        [JsonProperty("turma_id")]
        public long TurmaId { get; set; }
    }
}