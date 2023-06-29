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
        public bool LancaNota { get; set; }
        public bool PossuiObjetivos { get; set; }
        public bool Regencia { get; set; }
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
}