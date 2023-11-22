using Newtonsoft.Json;
using System.Linq;

namespace SME.SGP.Infra
{
    public class ProfessorTitularDisciplinaEol
    {

        [JsonProperty("disciplinas_Id")]
        public string CodigosDisciplinas { get; set; }
        public long[] DisciplinasId => string.IsNullOrEmpty(CodigosDisciplinas) ?
                                       Enumerable.Empty<long>().ToArray() :
                                       CodigosDisciplinas.Split(",").Select(x => long.Parse(x)).ToArray();

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