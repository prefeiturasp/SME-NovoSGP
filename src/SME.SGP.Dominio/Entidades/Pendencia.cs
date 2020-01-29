namespace SME.SGP.Dominio
{
    public class Pendencia : EntidadeBase
    {
        public Pendencia(string descricao, Fechamento fechamento, long fechamentoId, SituacaoPendencia situacao, TipoPendencia tipo, string titulo)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                throw new NegocioException("O título é obrigatório.");

            if (string.IsNullOrWhiteSpace(descricao))
                throw new NegocioException("A descrição é obrigatória.");
            if (fechamento == null)
                throw new NegocioException("O fehchamento é obrigatório.");

            Descricao = descricao;
            Fechamento = fechamento;
            FechamentoId = fechamentoId;
            Situacao = situacao;
            Tipo = tipo;
            Titulo = titulo;
        }

        protected Pendencia()
        {
        }

        public string Descricao { get; set; }
        public Fechamento Fechamento { get; set; }
        public long FechamentoId { get; set; }
        public SituacaoPendencia Situacao { get; set; }
        public TipoPendencia Tipo { get; set; }
        public string Titulo { get; set; }
    }
}