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
            Map(c => c.Modalidade).ToColumn("modalidade");
            Map(c => c.Semestre).ToColumn("semestre");
        }
    }

    
}