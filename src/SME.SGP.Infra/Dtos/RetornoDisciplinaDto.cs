using Newtonsoft.Json;
using SME.SGP.Dominio;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Infra
{
    public class RetornoDisciplinaDto
    {
        public long CdComponenteCurricular { get; set; }
        public long CodigoTerritorioSaber { get; set; }
        [JsonProperty("codDisciplinaPai")]
        public int? CdComponenteCurricularPai { get; set; }
        public string Descricao { get; set; }
        public bool EhCompartilhada { get; set; }
        public bool EhRegencia { get; set; }
        public bool RegistraFrequencia { get; set; }
        public bool Territorio { get; set; }
        public bool LancaNota { get; set; }
        public GrupoMatriz GrupoMatriz { get; set; }
        public bool BaseNacional { get; set; }
        public string Professor { get; set; }
    }

    public static class RetornoDisciplinaExtension
    {
        public static List<DisciplinaDto> MapearDto(this IEnumerable<RetornoDisciplinaDto> retornoDisciplinas)
        {
            return retornoDisciplinas.Select(x => new DisciplinaDto
            {
                CodigoComponenteCurricular = x.CdComponenteCurricular,
                CdComponenteCurricularPai = x.CdComponenteCurricularPai,
                CodigoTerritorioSaber = x.CodigoTerritorioSaber,
                Nome = x.Descricao,
                Regencia = x.EhRegencia,
                Compartilhada = x.EhCompartilhada,
                RegistraFrequencia = x.RegistraFrequencia,
                TerritorioSaber = x.Territorio,
                LancaNota = x.LancaNota,
                GrupoMatrizId = x.GrupoMatriz?.Id ?? 0,
                GrupoMatrizNome = x.GrupoMatriz?.Nome,
                Professor = x.Professor
            }).ToList();
        }
    }
}