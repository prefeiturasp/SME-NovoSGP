using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class RecuperacaoParalelaMap : DommelEntityMap<RecuperacaoParalela>
    {
        public RecuperacaoParalelaMap()
        {
            ToTable("recuperacao_paralela");
            Map(c => c.TurmaId).ToColumn("turma_id");
        }
    }
}