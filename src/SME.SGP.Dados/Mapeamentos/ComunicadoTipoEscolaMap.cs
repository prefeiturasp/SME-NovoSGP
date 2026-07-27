using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ComunicadoTipoEscolaMap : SimpleEntityMap<ComunicadoTipoEscola>
    {
        public ComunicadoTipoEscolaMap()
        {
            ToTable("comunicado_tipo_escola");
            Map(nameof(ComunicadoTipoEscola.ComunicadoId),"comunicado_id");
            Map(nameof(ComunicadoTipoEscola.TipoEscola),"tipo_escola");
        }
    }
}
