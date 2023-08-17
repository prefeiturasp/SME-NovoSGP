namespace SME.SGP.Dominio
{
    public class PlanoAEETurmaAluno : EntidadeBase
    {
        public long PlanoAEEId { get; set; }
        public long TurmaId { get; set; }
        public string AlunoCodigo { get; set; }
    }
}
