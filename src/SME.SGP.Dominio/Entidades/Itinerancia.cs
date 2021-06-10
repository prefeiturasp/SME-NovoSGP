using SME.SGP.Dominio.Enumerados;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Dominio
{
    public class Itinerancia : EntidadeBase
    {
        public Itinerancia()
        {
            alunos = new List<ItineranciaAluno>();
            questoes = new List<ItineranciaQuestao>();
            objetivos = new List<ItineranciaObjetivo>();
            objetivosBase = new List<ItineranciaObjetivoBase>();
            Situacao = SituacaoItinerancia.Digitado;
        }

        public long DreId { get; set; }
        public long UeId { get; set; }
        public Evento Evento { get; set; }
        public long? EventoId { get; set; }
        public DateTime DataVisita { get; set; }
        public int AnoLetivo { get; set; }
        public IEnumerable<ItineranciaObjetivo> ObjetivosVisita { get { return objetivos; } }
        public IEnumerable<ItineranciaAluno> Alunos { get { return alunos; } }
        public DateTime? DataRetornoVerificacao { get; set; }
        public IEnumerable<ItineranciaQuestao> Questoes { get { return questoes; } }
        public IEnumerable<ItineranciaObjetivoBase> ObjetivosBase { get { return objetivosBase; } }
        public SituacaoItinerancia Situacao { get; set; }

        private List<ItineranciaAluno> alunos { get; set; }
        private List<ItineranciaQuestao> questoes { get; set; }
        private List<ItineranciaObjetivo> objetivos { get; set; }
        private List<ItineranciaObjetivoBase> objetivosBase { get; set; }

        public void AdicionarAluno(ItineranciaAluno aluno)
        {
            if (aluno == null)
                throw new NegocioException("Não é possível incluir um aluno sem informação");

            if (!alunos.Any(a => a.Id == aluno.Id))
                alunos.Add(aluno);            
        }

        public void AdicionarQuestaoAluno(long alunoId, ItineranciaAlunoQuestao itineranciaAlunoQuestao)
        {
            var aluno = alunos.FirstOrDefault(a => a.Id == alunoId);
            if (aluno == null)
                throw new NegocioException($"Não foi possível localizar o nível de Id {alunoId}");

            if (!aluno.AlunosQuestoes.Any(q => q.Id == itineranciaAlunoQuestao.Id))
                aluno.Adicionar(itineranciaAlunoQuestao);            
        }

        public void AdicionarQuestao(ItineranciaQuestao questao)
        {
            if (!questoes.Any(q => q.Id == questao.Id))
                questoes.Add(questao);            
        }

        public void AdicionarObjetivo(ItineranciaObjetivo objetivo)
        {
            if (!objetivos.Any(o => o.Id == objetivo.Id))
                objetivos.Add(objetivo);
        }

        public void AdicionarObjetivoBase(ItineranciaObjetivoBase objetivoBase)
        {
            if (!objetivosBase.Any(o => o.Id == objetivoBase.Id))
                objetivosBase.Add(objetivoBase);
        }

        public bool PossuiAlunos()
            => Alunos != null && Alunos.Any();

        public bool PossuiObjetivos()
            => ObjetivosVisita != null && ObjetivosVisita.Any();

        public bool PossuiQuestoes()
            => Questoes == null && Questoes.Any();
    }
}
