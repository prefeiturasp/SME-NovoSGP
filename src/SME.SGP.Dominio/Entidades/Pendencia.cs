namespace SME.SGP.Dominio
{
    public class Pendencia : EntidadeBase
    {
        public Pendencia(string titulo, string descricao, TipoPendencia tipo)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                throw new NegocioException("O título é obrigatório.");

            if (string.IsNullOrWhiteSpace(descricao))
                throw new NegocioException("A descrição é obrigatória.");

            Descricao = descricao;
            Situacao = SituacaoPendencia.Pendente;
            Tipo = tipo;
            Titulo = titulo;
        }

        protected Pendencia()
        {
        }

        public string Descricao { get; set; }
        public SituacaoPendencia Situacao { get; set; }
        public TipoPendencia Tipo { get; set; }
        public string Titulo { get; set; }
    }
}