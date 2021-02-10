using System;
using System.Collections.Generic;

namespace SME.SGP.Dominio
{
    public class Itinerancia : EntidadeBase
    {
        public Itinerancia()
        {
            ObjetivosVisita = new List<ItineranciaObjetivo>();
            Ues = new List<ItineranciaUe>();
            Alunos = new List<ItineranciaAluno>();
            Questoes = new List<ItineranciaQuestao>();
        }
        public DateTime DataVisita { get; set; }
        public IEnumerable<ItineranciaObjetivo> ObjetivosVisita { get; set; }
        public IEnumerable<ItineranciaUe> Ues { get; set; }
        public IEnumerable<ItineranciaAluno> Alunos { get; set; }
        public DateTime DataRetornoVerificacao { get; set; }
        public IEnumerable<ItineranciaQuestao> Questoes { get; set; }


        public void AdicionarObjetivos(long nivelId, Notificacao notificacao, Usuario usuario)
        {
            var nivel = niveis.FirstOrDefault(a => a.Id == nivelId);
            if (nivel == null)
                throw new NegocioException($"Não foi possível localizar o nível de Id {nivelId}");

            notificacao.Usuario = usuario;

            nivel.Adicionar(notificacao);
        }

    }
}
