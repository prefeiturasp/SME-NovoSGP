using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class FiltroFeriadoCalendarioDto
    {
        public int Abrangencia { get; set; }
        public int Ano { get; set; }
        public string Nome { get; set; }
        public TipoFeriadoCalendario Tipo { get; set; }
    }
}