namespace SME.SGP.Dominio
{
    public class TipoAtividadeAvaliativa : EntidadeBase
    {
        public string Descricao { get; set; }
        public bool Excluido { get; set; }
        public string Nome { get; set; }
        public bool Situacao { get; set; }

        public void Excluir()
        {
            if (Excluido)
                throw new NegocioException("Esse tipo de avaliação já está excluida.");
            Excluido = true;
        }
    }
}