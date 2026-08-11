using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ComunicadoAnoEscolarMap : SimpleMap<ComunicadoAnoEscolar>
    {
        public ComunicadoAnoEscolarMap()
        {
            ToTable("comunicado_ano_escolar");
            Map(nameof(ComunicadoAnoEscolar.ComunicadoId), "comunicado_id");
            Map(nameof(ComunicadoAnoEscolar.AnoEscolar), "ano_escolar");
        }
    }
}
