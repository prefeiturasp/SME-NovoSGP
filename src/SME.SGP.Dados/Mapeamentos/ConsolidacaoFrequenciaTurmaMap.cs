using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConsolidacaoFrequenciaTurmaMap : DommelEntityMap<ConsolidacaoFrequenciaTurma>
    {
        public ConsolidacaoFrequenciaTurmaMap()
        {
            ToTable("consolidacao_frequencia_turma");
            Map(c => c.Id).ToColumn("id");
            Map(c => c.TurmaId).ToColumn("turma_id");
            Map(c => c.QuantidadeAcimaMinimoFrequencia).ToColumn("quantidade_acima_minimo_frequencia");
            Map(c => c.QuantidadeAbaixoMinimoFrequencia).ToColumn("quantidade_abaixo_minimo_frequencia");
        }
    }

    
}