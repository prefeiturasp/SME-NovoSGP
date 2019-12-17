namespace SME.SGP.Dominio
{
    public class AtividadeAvaliativaDisciplina : EntidadeBase
    {
        public long AtividadeAvaliativaId { get; set; }
        public long DisciplinaId { get; set; }
        public bool Excluido { get; set; }

        public void Excluir()
        {
            Excluido = true;
        }
    }
}
