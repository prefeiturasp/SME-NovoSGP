﻿using Newtonsoft.Json;

namespace SME.SGP.Aplicacao.Integracoes.Respostas
{
    public class DisciplinaResposta
    {
        [JsonProperty("codDisciplina")]
        public long CodigoComponenteCurricular { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("compartilhada")]
        public bool Compartilhada { get; set; }

        [JsonProperty("codDisciplinaPai")]
        public long? CodigoComponenteCurricularPai { get; set; }
        [JsonProperty("CodCompTerritorioSaber")]
        public long? CodigoComponenteTerritorioSaber { get; set; }

        [JsonProperty("disciplina")]
        public string Nome { get; set; }

        [JsonProperty("regencia")]
        public bool Regencia { get; set; }

        [JsonProperty("registrofrequencia")]
        public bool RegistroFrequencia { get; set; }

        [JsonProperty("territoriosaber")]
        public bool TerritorioSaber { get; set; }

        [JsonProperty("lancaNota")]
        public bool LancaNota { get; set; }

        [JsonProperty("baseNacional")]
        public bool BaseNacional { get; set; }

        [JsonProperty("grupoMatriz")]
        public GrupoMatriz GrupoMatriz { get; set; }
        public string TurmaCodigo { get; internal set; }

        [JsonProperty("nomeComponenteInfantil")]
        public string NomeComponenteInfantil { get; internal set; }
        [JsonIgnore]
        public string Professor { get; set; }
    }
}