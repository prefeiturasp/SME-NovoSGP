namespace SME.SGP.Infra
{
    public class EventosAulasTipoDiaDto
    {
        public DadosAulaDto DadosAula { get; set; }
        public string Descricao { get; set; }
        public long Id { get; set; }
        public string TipoEvento { get; set; }
    }
}