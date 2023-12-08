using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Dominio
{
    public class ComponenteCurricularEol
    {
        public long Codigo { get; set; }
        public long? CodigoComponenteCurricularPai { get; set; }
        public long CodigoComponenteTerritorioSaber { get; set; }
        public bool Compartilhada { get; set; }
        public string Descricao { get; set; }
        public string DescricaoComponenteInfantil { get; set; }
        public bool LancaNota { get; set; }
        public bool PossuiObjetivos { get; set; }
        public bool Regencia { get; set; }
        public bool PlanejamentoRegencia { get; set; }
        public bool RegistraFrequencia { get; set; }
        public bool TerritorioSaber { get; set; }
        public bool BaseNacional { get; set; }
        public bool ExibirComponenteEOL { get; set; }
        public GrupoMatriz GrupoMatriz { get; set; }
        public string TurmaCodigo { get; set; }
        public string Professor { get; set; }
        public long[] CodigosTerritoriosAgrupamento { get; set; }

        public bool PossuiObjetivosDeAprendizagem(IEnumerable<ComponenteCurricularJurema> componentesCurricularesJurema, Modalidade turmaModalidade)
        {
            if(new[] { Modalidade.EJA, Modalidade.Medio }.Contains(turmaModalidade))
                return false;

            return componentesCurricularesJurema.Any(x => x.CodigoEOL == Codigo);
        }

        public bool PossuiObjetivosDeAprendizagemOpcionais(IEnumerable<ComponenteCurricularJurema> componentesCurricularesJurema, bool ensinoEspecial)
        {
            return ensinoEspecial && (componentesCurricularesJurema.Any(x => x.CodigoEOL == Codigo && new long[] { 218, 138, 1116 }.Contains(Codigo)) || Regencia);
        }
    }

    public static class ComponenteCurricularEolExtension
    {
        public static long[] ObterCodigos(this IEnumerable<ComponenteCurricularEol> componentesCurriculares)
        {
            var codigosComponentes = componentesCurriculares.Select(cc => cc.Codigo).ToList();
            codigosComponentes.AddRange(componentesCurriculares.Select(cc => cc.CodigoComponenteTerritorioSaber).Where(cc => cc != 0).ToList());
            return codigosComponentes.ToArray();
        }

        public static void PreencherInformacoesPegagogicasSgp(this List<ComponenteCurricularEol> disciplinasReposta, IEnumerable<InfoComponenteCurricular> componentesCurricularesSgp)
        {
            disciplinasReposta.ForEach(componenteCurricular =>
            {
                var componenteCurricularSgp = componentesCurricularesSgp.FirstOrDefault(cc => cc.Codigo == componenteCurricular.Codigo
                                                                                    || (componenteCurricular.CodigoComponenteTerritorioSaber != 0 &&
                                                                                        cc.Codigo == componenteCurricular.CodigoComponenteTerritorioSaber));

                if (componenteCurricularSgp.NaoEhNulo())
                {
                    componenteCurricular.GrupoMatriz = new GrupoMatriz() { Id = componenteCurricularSgp.GrupoMatrizId, Nome = componenteCurricularSgp.GrupoMatrizNome };
                    componenteCurricular.LancaNota = componenteCurricularSgp.LancaNota;
                    componenteCurricular.RegistraFrequencia = componenteCurricularSgp.RegistraFrequencia;
                    componenteCurricular.Compartilhada = componenteCurricularSgp.EhCompartilhada;
                    componenteCurricular.BaseNacional = componenteCurricularSgp.EhBaseNacional;
                    componenteCurricular.Regencia = componenteCurricularSgp.EhRegencia || componenteCurricular.PlanejamentoRegencia;
                    componenteCurricular.TerritorioSaber = componenteCurricularSgp.EhTerritorioSaber;

                    if (!componenteCurricular.TerritorioSaber)
                    {
                        componenteCurricular.Descricao = componenteCurricularSgp.Nome;
                        componenteCurricular.DescricaoComponenteInfantil = componenteCurricularSgp.NomeComponenteInfantil;
                    }
                }
            });
        }
    }
}