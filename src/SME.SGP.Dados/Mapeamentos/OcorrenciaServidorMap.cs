using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class OcorrenciaServidorMap : DommelEntityMap<OcorrenciaServidor>
    {
        public OcorrenciaServidorMap()
        {
            ToTable("ocorrencia_servidor");
            Map(c => c.Id).ToColumn("id").IsIdentity().IsKey();
            Map(c => c.CodigoServidor).ToColumn("rf_codigo");
            Map(c => c.OcorrenciaId).ToColumn("ocorrencia_id");
        }
    }
}
