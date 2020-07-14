using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SME.SGP.Dominio
{
    public class ComponenteCurricularEol
    {
        public long Codigo { get; set; }
        public long? CodigoComponenteCurricularPai { get; set; }
        public bool Compartilhada { get; set; }
        public string Descricao { get; set; }
        public bool LancaNota { get; set; }
        public bool PossuiObjetivos { get; set; }
        public bool Regencia { get; set; }
        public bool RegistraFrequencia { get; set; }
        public bool TerritorioSaber { get; set; }
        public bool BaseNacional { get; set; }
        public GrupoMatriz GrupoMatriz { get; set; }

        public bool PossuiObjetivosDeAprendizagem(IEnumerable<ComponenteCurricular> componentesCurricularesJurema, bool turmaPrograma, Modalidade turmaModalidade, string turmaAno)
        {
            var posuiObjetivos = componentesCurricularesJurema.Any(x => x.CodigoEOL == Codigo) && !turmaPrograma &&
                    !new[] { Modalidade.EJA, Modalidade.Medio }.Contains(turmaModalidade) && turmaAno != "0";

            return posuiObjetivos;
        }

        public bool PossuiObjetivosDeAprendizagemOpcionais(IEnumerable<ComponenteCurricular> componentesCurricularesJurema, bool ensinoEspecial)
        {
            return ensinoEspecial && (componentesCurricularesJurema.Any(x => x.CodigoEOL == Codigo && new long[] { 218, 138, 1116 }.Contains(Codigo)) || Regencia);
        }
    }
}