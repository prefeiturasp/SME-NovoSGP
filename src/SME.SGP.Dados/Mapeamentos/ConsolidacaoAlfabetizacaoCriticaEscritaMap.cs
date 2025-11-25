using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio.Entidades;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConsolidacaoAlfabetizacaoCriticaEscritaMap : DommelEntityMap<ConsolidacaoAlfabetizacaoCriticaEscrita>
    {
        public ConsolidacaoAlfabetizacaoCriticaEscritaMap()
        {
            ToTable("consolidacao_alfabetizacao_critica_escrita");
            Map(c => c.Id).ToColumn("id").IsIdentity().IsKey();
            Map(c => c.DreCodigo).ToColumn("dre_codigo");
            Map(c => c.UeCodigo).ToColumn("ue_codigo");
            Map(c => c.DreNome).ToColumn("dre_nome");
            Map(c => c.UeNome).ToColumn("ue_nome");
            Map(c => c.Posicao).ToColumn("posicao");
            Map(c => c.TotalAlunosNaoAlfabetizados).ToColumn("total_alunos_nao_alfabetizados");
            Map(c => c.PercentualTotalAlunos).ToColumn("percentual_total_alunos");
            Map(c => c.AnoLetivo).ToColumn("ano_letivo");
        }
    }
}