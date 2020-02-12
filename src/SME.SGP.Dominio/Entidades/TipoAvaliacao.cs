namespace SME.SGP.Dominio
{
    public class TipoAvaliacao : EntidadeBase
    {
        public int AvaliacoesNecessariasPorBimestre { get; set; }
        public string Descricao { get; set; }
        public bool Excluido { get; set; }
        public string Nome { get; set; }
        public bool Situacao { get; set; }
        public TipoAvaliacaoCodigo Codigo { get; set; }

        public void Excluir()
        {
            if (Excluido)
                throw new NegocioException("Esse tipo de avaliação já está excluida.");
            Excluido = true;
        }
    }
}