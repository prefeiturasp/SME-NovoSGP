using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ComunicadoAnoEscolarMap : DommelEntityMap<ComunicadoAnoEscolar>
    {
        public ComunicadoAnoEscolarMap()
        {
            ToTable("comunicado_ano_escolar");
            Map(c => c.ComunicadoId).ToColumn("comunicado_id");
            Map(c => c.AnoEscolar).ToColumn("ano_escolar");
        }
    }
}
