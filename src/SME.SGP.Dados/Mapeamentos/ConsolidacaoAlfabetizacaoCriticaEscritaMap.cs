using SME.SGP.Dominio.Entidades;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConsolidacaoAlfabetizacaoCriticaEscritaMap : SimpleEntityMap<ConsolidacaoAlfabetizacaoCriticaEscrita>
    {
        public ConsolidacaoAlfabetizacaoCriticaEscritaMap()
        {
            ToTable("consolidacao_alfabetizacao_critica_escrita");
            Map(nameof(ConsolidacaoAlfabetizacaoCriticaEscrita.DreCodigo),"dre_codigo");
            Map(nameof(ConsolidacaoAlfabetizacaoCriticaEscrita.UeCodigo),"ue_codigo");
            Map(nameof(ConsolidacaoAlfabetizacaoCriticaEscrita.DreNome),"dre_nome");
            Map(nameof(ConsolidacaoAlfabetizacaoCriticaEscrita.UeNome),"ue_nome");
            Map(nameof(ConsolidacaoAlfabetizacaoCriticaEscrita.Posicao),"posicao");
            Map(nameof(ConsolidacaoAlfabetizacaoCriticaEscrita.TotalAlunosNaoAlfabetizados),"total_alunos_nao_alfabetizados");
            Map(nameof(ConsolidacaoAlfabetizacaoCriticaEscrita.PercentualTotalAlunos),"percentual_total_alunos");
        }
    }
}