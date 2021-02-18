using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Dominio
{
    public class ItineranciaAluno : EntidadeBase
    {
        public ItineranciaAluno()
        {
            questoes = new List<ItineranciaAlunoQuestao>();
        }

        public string CodigoAluno { get; set; }
        public long ItineranciaId { get; set; }
        public long TurmaId { get; set; }
        public IEnumerable<ItineranciaAlunoQuestao> AlunosQuestoes { get { return questoes; } }
        public bool Excluido { get; set; }

        private List<ItineranciaAlunoQuestao> questoes { get; set; }

        public void Adicionar(ItineranciaAlunoQuestao questao)
        {
            if (questao == null)
                throw new NegocioException("Não é possível incluir uma questão sem informação");

            if (!questoes.Any(q => q.Id == questao.Id))
                questoes.Add(questao);
        }
    }
}