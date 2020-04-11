using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ComunicadoGrupoMap : BaseMap<ComunicadoGrupo>
    {
        public ComunicadoGrupoMap()
        {
            ToTable("comunidado_grupo");
            Map(c => c.ComunicadoId).ToColumn("comunicado_id");
            Map(c => c.GrupoComunicadoId).ToColumn("grupo_comunicado_id");
        }
    }
}