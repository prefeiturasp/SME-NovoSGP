using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ComunicadoModalidadeMap : DommelEntityMap<ComunicadoModalidade>
    {
        public ComunicadoModalidadeMap()
        {
            ToTable("comunicado_modalidade");
            Map(c => c.ComunicadoId).ToColumn("comunicado_id");
        }
    }
}
