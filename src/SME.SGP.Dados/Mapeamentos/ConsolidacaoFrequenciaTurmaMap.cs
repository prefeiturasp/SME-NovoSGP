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
            Map(c => c.AnoLetivo).ToColumn("ano_letivo");
            Map(c => c.Semestre).ToColumn("semestre");
            Map(c => c.CodigoDre).ToColumn("codigo_dre");
            Map(c => c.SiglaDre).ToColumn("sigla_dre"); 
            Map(c => c.CodigoUe).ToColumn("codigo_ue");
            Map(c => c.SiglaUe).ToColumn("sigla_ue");
            Map(c => c.CodigoTurma).ToColumn("codigo_turma");
            Map(c => c.QuantidadeAcimaMinimoFrequencia).ToColumn("quantidade_acima_minimo_frequencia");
            Map(c => c.QuantidadeAbaixoMinimoFrequencia).ToColumn("quantidade_abaixo_minimo_frequencia");

        }
    }

    
}