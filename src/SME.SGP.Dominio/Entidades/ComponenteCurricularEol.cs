
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

        public bool AtribuicaoAtiva { get; set; }
        public DateTime? InicioAtribuicao { get; set; }
        public DateTime? FimAtribuicao { get; set; }

        public bool AtribuicaoAlcancaData(DateTime data)
        {
            if (AtribuicaoAtiva || (!InicioAtribuicao.HasValue && !FimAtribuicao.HasValue))
                return true;

            var dataReferencia = data.Date;

            return (!InicioAtribuicao.HasValue || InicioAtribuicao.Value.Date <= dataReferencia)
                && (!FimAtribuicao.HasValue || FimAtribuicao.Value.Date >= dataReferencia);
        }

        public bool PossuiObjetivosDeAprendizagem(IEnumerable<ComponenteCurricularJurema> componentesCurricularesJurema, Modalidade turmaModalidade)
        => turmaModalidade.PossuiObjetivosAprendizagem()
           && componentesCurricularesJurema.Any(x => x.CodigoEOL == Codigo);
        
        public bool PossuiObjetivosDeAprendizagemOpcionais(IEnumerable<ComponenteCurricularJurema> componentesCurricularesJurema, bool ensinoEspecial)
        {
            return ensinoEspecial && (componentesCurricularesJurema.Any(x => x.CodigoEOL == Codigo && new long[] { 218, 138, 1116 }.Contains(Codigo)) || Regencia);
        }
    }

    public static class ComponenteCurricularEolExtension
    {
        public static bool PossuiCodigoEquivalente(this ComponenteCurricularEol componenteCurricular, long codigoComponenteCurricular)
        {
            if (componenteCurricular.EhNulo())
                return false;

            return componenteCurricular.ObterCodigosEquivalentes()
                .Contains(codigoComponenteCurricular);
        }

        public static IEnumerable<long> ObterCodigosEquivalentes(this ComponenteCurricularEol componenteCurricular)
        {
            if (componenteCurricular.EhNulo())
                return Enumerable.Empty<long>();

            var codigos = new List<long> { componenteCurricular.Codigo };

            if (componenteCurricular.CodigoComponenteCurricularPai.HasValue &&
                componenteCurricular.CodigoComponenteCurricularPai.Value > 0)
                codigos.Add(componenteCurricular.CodigoComponenteCurricularPai.Value);

            if (componenteCurricular.CodigoComponenteTerritorioSaber > 0)
                codigos.Add(componenteCurricular.CodigoComponenteTerritorioSaber);

            if (componenteCurricular.CodigosTerritoriosAgrupamento.NaoEhNulo())
                codigos.AddRange(componenteCurricular.CodigosTerritoriosAgrupamento.Where(codigo => codigo > 0));

            return codigos.Distinct();
        }

        public static string[] ObterCodigosEquivalentes(this IEnumerable<ComponenteCurricularEol> componentesCurriculares)
        {
            if (componentesCurriculares.EhNulo())
                return Array.Empty<string>();

            return componentesCurriculares
                .SelectMany(componenteCurricular => componenteCurricular.ObterCodigosEquivalentes())
                .Distinct()
                .Select(codigo => codigo.ToString())
                .ToArray();
        }

        public static long[] ObterCodigos(this IEnumerable<ComponenteCurricularEol> componentesCurriculares)
        {
            var codigosComponentes = componentesCurriculares.Select(cc => cc.Codigo).ToList();
            codigosComponentes.AddRange(componentesCurriculares.Select(cc => cc.CodigoComponenteTerritorioSaber).Where(cc => cc != 0).ToList());
            return codigosComponentes.ToArray();
        }

        public static void PreencherInformacoesPegagogicasSgp(this List<ComponenteCurricularEol> disciplinasReposta, IEnumerable<InfoComponenteCurricular> componentesCurricularesSgp)
        {
            var componentesEquivalentes = from dr in disciplinasReposta
                                          join ccsgp in componentesCurricularesSgp
                                             on (dr.TerritorioSaber && dr.CodigoComponenteTerritorioSaber != 0 ? dr.CodigoComponenteTerritorioSaber : dr.Codigo) equals ccsgp.Codigo
                                          select (dr, ccsgp);

            foreach (var (dr, ccsgp) in componentesEquivalentes)
            {
                if (ccsgp.NaoEhNulo())
                {
                    dr.GrupoMatriz = new GrupoMatriz() { Id = ccsgp.GrupoMatrizId, Nome = ccsgp.GrupoMatrizNome };
                    dr.LancaNota = ccsgp.LancaNota;
                    dr.RegistraFrequencia = ccsgp.RegistraFrequencia;
                    dr.Compartilhada = ccsgp.EhCompartilhada;
                    dr.BaseNacional = ccsgp.EhBaseNacional;
                    dr.Regencia = ccsgp.EhRegencia || dr.PlanejamentoRegencia;
                    dr.TerritorioSaber = ccsgp.EhTerritorioSaber;

                    if (!dr.TerritorioSaber)
                    {
                        dr.Descricao = ccsgp.Nome;
                        dr.DescricaoComponenteInfantil = ccsgp.NomeComponenteInfantil;
                    }
                }
            }
        }
    }
}