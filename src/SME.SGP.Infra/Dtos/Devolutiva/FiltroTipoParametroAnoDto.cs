using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class FiltroTipoParametroAnoDto
    {
        public FiltroTipoParametroAnoDto(TipoParametroSistema tipoParametro, int anoLetivo)
        {
            TipoParametro = tipoParametro;
            AnoLetivo = anoLetivo;
        }
        public TipoParametroSistema TipoParametro { get; set; }
        public int AnoLetivo { get; set; }
    }
}
