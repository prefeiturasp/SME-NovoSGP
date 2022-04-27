namespace SME.SGP.Dominio
{
    public class ConselhoClasseRecomendacao : EntidadeBase
    {
        public bool Excluido { get; set; }
        public string Recomendacao { get; set; }
        public ConselhoClasseRecomendacaoTipo Tipo { get; set; }

        public ConselhoClasseRecomendacao()
        {

        }
    }
}