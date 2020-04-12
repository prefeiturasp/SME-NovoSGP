using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ComunicadoGrupoMap : DommelEntityMap<ComunicadoGrupo>
    {
        public ComunicadoGrupoMap()
        {
            ToTable("comunidado_grupo");
            Map(c => c.ComunicadoId).ToColumn("comunicado_id");
            Map(c => c.GrupoComunicadoId).ToColumn("grupo_comunicado_id");
        }
    }
}