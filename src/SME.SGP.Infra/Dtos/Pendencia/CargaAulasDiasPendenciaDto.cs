namespace SME.SGP.Infra
{
    public class CargaAulasDiasPendenciaDto
    {
        public CargaAulasDiasPendenciaDto(long pendenciaId, int quantidadeDias, int quantidadeAulas)
        {
            PendenciaId = pendenciaId;
            QuantidadeDias = quantidadeDias;
            QuantidadeAulas = quantidadeAulas;
        }

        public long PendenciaId { get; set; }
        public int QuantidadeDias { get; set; }
        public int QuantidadeAulas { get; set; }
    }
}