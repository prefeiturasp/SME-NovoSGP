using System.Text.Json.Serialization;

namespace SME.SGP.Infra
{
    public class TipoReponsavelRetornoDto
    {
        [JsonPropertyName("codigo")]
        public int Codigo { get; set; }
        [JsonPropertyName("descricao")]
        public string Descricao { get; set; }
    }
}
