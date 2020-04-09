namespace SME.SGP.Dominio
{
    public class GrupoComunicacao : EntidadeBase
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string TipoCicloId { get; set; }
        public string TipoEscolaId { get; set; }
    }
}