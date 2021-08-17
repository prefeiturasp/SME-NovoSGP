using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ComunicadoTipoEscolaMap : DommelEntityMap<ComunicadoTipoEscola>
    {
        public ComunicadoTipoEscolaMap()
        {
            ToTable("comunicado_tipo_escola");
            Map(c => c.ComunicadoId).ToColumn("comunicado_id");
            Map(c => c.TipoEscola).ToColumn("tipo_escola");
        }
    }
}
