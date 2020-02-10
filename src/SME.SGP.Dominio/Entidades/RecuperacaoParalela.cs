namespace SME.SGP.Dominio
{
    public class RecuperacaoParalela : EntidadeBase
    {
        public long Aluno_id { get; set; }
        public bool Excluido { get; set; }
        public string TurmaId { get; set; }
        public string TurmaRecuperacaoParalelaId { get; set; }
    }
}