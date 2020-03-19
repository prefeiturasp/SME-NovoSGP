namespace SME.SGP.Dominio
{
    public class FechamentoTurmaDisciplina : EntidadeBase
    {
        protected FechamentoTurmaDisciplina() { }
        public FechamentoTurmaDisciplina(long turmaId, long disciplinaId, long periodoEscolarId)
        {
            TurmaId = turmaId;
            DisciplinaId = disciplinaId;
            PeriodoEscolarId = periodoEscolarId;
        }

        public long PeriodoEscolarId { get; set; }
        public PeriodoEscolar PeriodoEscolar { get; set; }
        public long TurmaId { get; set; }
        public Turma Turma { get; set; }
        public long DisciplinaId { get; set; }
        public SituacaoFechamento Situacao { get; set; }
        public string Justificativa { get; set; }

        public bool Migrado { get; set; }
        public bool Excluido { get; set; }

        public void AdicionarPeriodoEscolar(PeriodoEscolar periodoEscolar)
        {
            PeriodoEscolar = periodoEscolar;
        }

        public void AtualizarSituacao(SituacaoFechamento processadoComPendencias)
        {
            Situacao = processadoComPendencias;
        }
    }
}