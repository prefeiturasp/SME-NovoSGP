namespace SME.SGP.Dominio
{
    public class AtividadeAvaliativaDisciplina : EntidadeBase
    {
        public AtividadeAvaliativaDisciplina() { }
        public AtividadeAvaliativaDisciplina(long atividadeAvaliativaId, string disciplinaId)
        {
            AtividadeAvaliativaId = atividadeAvaliativaId;
            DisciplinaId = disciplinaId;
        }

        public long AtividadeAvaliativaId { get; set; }
        public string DisciplinaId { get; set; }
        public bool Excluido { get; set; }

        public void Excluir()
        {
            Excluido = true;
        }
    }
}
