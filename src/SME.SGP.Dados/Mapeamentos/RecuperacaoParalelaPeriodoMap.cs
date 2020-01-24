using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class RecuperacaoParalelaPeriodoMap : DommelEntityMap<RecuperacaoParalelaPeriodo>
    {
        public RecuperacaoParalelaPeriodoMap()
        {
            ToTable("recuperacao_paralela_periodo");
            Map(c => c.Nome).ToColumn("nome");
            Map(c => c.Descricao).ToColumn("descricao");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}