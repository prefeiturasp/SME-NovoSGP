using System;
using System.Collections.Generic;

namespace SME.SGP.Dominio
{
    public class Comunicado : EntidadeBase
    {
        public Comunicado()
        {
            Grupos = new List<GrupoComunicacao>();
        }

        public DateTime DataEnvio { get; set; }
        public DateTime? DataExpiracao { get; set; }
        public string Descricao { get; set; }
        public bool Excluido { get; set; }
        public List<GrupoComunicacao> Grupos { get; set; }
        public string Titulo { get; set; }

        public void AdicionarGrupo(GrupoComunicacao grupo)
        {
            Grupos.Add(grupo);
        }

        public void MarcarExcluido()
        {
            Excluido = true;
        }
    }
}