using Newtonsoft.Json;
using SME.SGP.Dominio;
using System.Collections.Generic;
using System.Linq;

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
        [JsonProperty("professor")]
        public string Professor { get; set; }
        [JsonProperty("codigosTerritoriosAgrupamento")]
        public long[] CodigosTerritoriosAgrupamento { get; set; }
    }

    public static class DisciplinaRespostaExtension
    {
        public static long[] ObterCodigos(this IEnumerable<DisciplinaResposta> disciplinasReposta)
        {
            var codigosComponentes = disciplinasReposta.Select(cc => cc.CodigoComponenteCurricular).ToList();
            codigosComponentes.AddRange(disciplinasReposta.Select(cc => cc.CodigoComponenteTerritorioSaber ?? 0).Where(cc => cc != 0).ToList());
            return codigosComponentes.ToArray();
        }
        
        public static void PreencherInformacoesPegagogicasSgp(this List<DisciplinaResposta> disciplinasReposta, IEnumerable<InfoComponenteCurricular> componentesCurricularesSgp)
        {
            disciplinasReposta.ForEach(componenteCurricular =>
            {
                var componenteCurricularSgp = componentesCurricularesSgp.Where(cc => cc.Codigo == componenteCurricular.CodigoComponenteCurricular
                                                                                    || (componenteCurricular.CodigoComponenteTerritorioSaber != 0 &&
                                                                                        cc.Codigo == componenteCurricular.CodigoComponenteTerritorioSaber)).FirstOrDefault();

                if (componenteCurricularSgp != null)
                {
                    componenteCurricular.GrupoMatriz = new GrupoMatriz() { Id = componenteCurricularSgp.GrupoMatrizId, Nome = componenteCurricularSgp.GrupoMatrizNome };
                    componenteCurricular.LancaNota = componenteCurricularSgp.LancaNota;
                    componenteCurricular.RegistroFrequencia = componenteCurricularSgp.RegistraFrequencia;
                    componenteCurricular.Compartilhada = componenteCurricularSgp.EhCompartilhada;
                    componenteCurricular.BaseNacional = componenteCurricularSgp.EhBaseNacional;
                    componenteCurricular.Regencia = componenteCurricularSgp.EhRegencia;
                    componenteCurricular.TerritorioSaber = componenteCurricularSgp.EhTerritorioSaber;

                    var naoEhComponenteTerritorioExtenso = (componenteCurricular.CodigoComponenteTerritorioSaber ?? 0) == 0 ||
                                                            componenteCurricular.CodigoComponenteCurricular == (componenteCurricular.CodigoComponenteTerritorioSaber ?? 0);
                    if (naoEhComponenteTerritorioExtenso)
                    {
                        componenteCurricular.Nome = componenteCurricularSgp.Nome;
                        componenteCurricular.NomeComponenteInfantil = componenteCurricularSgp.NomeComponenteInfantil;
                    }
                }
            });
        }
    }
}