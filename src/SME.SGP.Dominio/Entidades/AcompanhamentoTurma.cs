namespace SME.SGP.Dominio
{
    public class AcompanhamentoTurma : EntidadeBase
    {
        public Turma Turma { get; set; }
        public long TurmaId { get; set; }

        public int Semestre { get; set; }
        public string ApanhadoGeral { get; set; }
    }
}
