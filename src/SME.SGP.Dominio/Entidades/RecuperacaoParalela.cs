namespace SME.SGP.Dominio
{
    public class RecuperacaoParalela : EntidadeBase
    {
        public long Aluno_id { get; set; }
        public bool Excluido { get; set; }
        public long TurmaId { get; set; }
        public long TurmaRecuperacaoParalelaId { get; set; }
        public int AnoLetivo { get; set; }
    }
}