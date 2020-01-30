namespace SME.SGP.Dominio
{
    public class Pendencia : EntidadeBase
    {
        public Pendencia(string titulo, string descricao, long fechamentoId, TipoPendencia tipo = TipoPendencia.Fechamento)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                throw new NegocioException("O título é obrigatório.");

            if (string.IsNullOrWhiteSpace(descricao))
                throw new NegocioException("A descrição é obrigatória.");

            Descricao = descricao;
            FechamentoId = fechamentoId;
            Situacao = SituacaoPendencia.Pendente;
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