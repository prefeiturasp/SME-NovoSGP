using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ComunicadoMap : DommelEntityMap<Comunicado>
    {
        public ComunicadoMap()
        {
            ToTable("comunicado");
            Map(c => c.Id).ToColumn("Id");
            Map(c => c.AlteradoEm).ToColumn("alterado_em");
            Map(c => c.AlteradoPor).ToColumn("alterado_por");
            Map(c => c.AlteradoRF).ToColumn("alterado_rf");
            Map(c => c.CriadoEm).ToColumn("criado_em");
            Map(c => c.CriadoPor).ToColumn("criado_por");
            Map(c => c.CriadoRF).ToColumn("criado_rF");
            Map(c => c.Descricao).ToColumn("descricao");
            Map(c => c.Titulo).ToColumn("Titulo");
            Map(c => c.DataEnvio).ToColumn("data_envio");
            Map(c => c.DataExpiracao).ToColumn("data_expiracao");
        }
    }
}