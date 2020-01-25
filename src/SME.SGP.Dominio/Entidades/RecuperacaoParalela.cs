namespace SME.SGP.Dominio
{
    public class RecuperacaoParalela : EntidadeBase
    {
        public int Aluno_id { get; set; }
        public bool Excluido { get; set; }
        public int TurmaId { get; set; }
    }
}