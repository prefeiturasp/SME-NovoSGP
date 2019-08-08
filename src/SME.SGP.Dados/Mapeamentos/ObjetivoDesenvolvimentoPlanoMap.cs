using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ObjetivoDesenvolvimentoPlanoMap : DommelEntityMap<ObjetivoDesenvolvimentoPlano>
    {
        public ObjetivoDesenvolvimentoPlanoMap()
        {
            ToTable("objetivo_desenvolvimento_plano");
            Map(c => c.ObjetivoDesenvolvimentoId).ToColumn("objetivo_desenvolvimento_id");
            Map(c => c.PlanoId).ToColumn("plano_id");
        }
    }
}