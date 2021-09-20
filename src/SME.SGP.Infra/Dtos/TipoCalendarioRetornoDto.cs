using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class TipoCalendarioRetornoDto
    {
        public long Id { get; set; }
        public int AnoLetivo { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public ModalidadeTipoCalendario Modalidade { get; set; }
    }
}
