using System.Collections.Generic;

namespace SME.SGP.Dominio
{
    public class Fechamento : EntidadeBase
    {
        public Fechamento(string disciplinaId, long periodoEscolarId, SituacaoFechamento situacao, long turmaId)
        {
            DisciplinaId = disciplinaId;
            Pendencias = new List<Pendencia>();
            PeriodoEscolarId = periodoEscolarId;
            Situacao = situacao;
            TurmaId = turmaId;
        }

        protected Fechamento()
        {
            Pendencias = new List<Pendencia>();
        }

        public string DisciplinaId { get; set; }
        public IEnumerable<Pendencia> Pendencias { get; set; }
        public PeriodoEscolar PeriodoEscolar { get; set; }
        public long PeriodoEscolarId { get; set; }
        public SituacaoFechamento Situacao { get; set; }
        public Turma Turma { get; set; }
        public long TurmaId { get; set; }
    }
}