namespace SME.SGP.Dominio
{
    public class Pendencia : EntidadeBase
    {
        public Pendencia(TipoPendencia tipo, string titulo = "", string descricao = "")
        {
            Situacao = SituacaoPendencia.Pendente;
            Tipo = tipo;
            Titulo = titulo;
            Descricao = descricao;
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