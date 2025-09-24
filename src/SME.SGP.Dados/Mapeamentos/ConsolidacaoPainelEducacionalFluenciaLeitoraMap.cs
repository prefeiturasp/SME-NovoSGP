using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio.Entidades;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConsolidacaoPainelEducacionalFluenciaLeitoraMap : DommelEntityMap<ConsolidacaoPainelEducacionalFluenciaLeitora>
    {
        public ConsolidacaoPainelEducacionalFluenciaLeitoraMap()
        {
            ToTable("consolidacao_painel_educacional_fluencia_leitora");
            Map(c => c.Id).ToColumn("id").IsIdentity().IsKey();
            Map(c => c.Fluencia).ToColumn("fluencia");
            Map(c => c.DescricaoFluencia).ToColumn("descricao_fluencia");
            Map(c => c.DreCodigo).ToColumn("dre_codigo");
            Map(c => c.Percentual).ToColumn("percentual");
            Map(c => c.Ano).ToColumn("ano");
            Map(c => c.Periodo).ToColumn("periodo");
            Map(c => c.QuantidadeAlunos).ToColumn("quantidade_alunos");
        }
    }
}