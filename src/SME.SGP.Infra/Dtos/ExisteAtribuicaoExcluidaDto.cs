namespace SME.SGP.Infra
{
    public record ExisteAtribuicaoExcluidaDto
    {
        public long Id { get; set; }
        public string UeCodigo { get; set; }
        public string CriadoPor { get; set; }
        public string CriadoRF { get; set; }
    };
}
