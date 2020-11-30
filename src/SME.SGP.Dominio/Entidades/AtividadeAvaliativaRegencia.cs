namespace SME.SGP.Dominio
{
    public class AtividadeAvaliativaRegencia : EntidadeBase
    {
        public long AtividadeAvaliativaId { get; set; }
        public string DisciplinaContidaRegenciaId { get; set; }
        public string DisciplinaContidaRegenciaNome { get; set; }
        public bool Excluido { get; set; }

        public void Excluir()
        {
            Excluido = true;
        }
    }
}