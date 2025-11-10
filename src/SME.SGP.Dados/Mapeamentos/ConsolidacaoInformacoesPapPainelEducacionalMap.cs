using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConsolidacaoInformacoesPapMap : DommelEntityMap<ConsolidacaoInformacoesPap>
    {
        public ConsolidacaoInformacoesPapMap()
        {
            ToTable("consolidacao_informacoes_pap");
            Map(c => c.Id).ToColumn("id").IsIdentity().IsKey();
            Map(c => c.TipoPap).ToColumn("tipo_pap");
            Map(c => c.DreCodigo).ToColumn("dre_codigo");
            Map(c => c.DreNome).ToColumn("dre_nome");
            Map(c => c.UeCodigo).ToColumn("ue_codigo");
            Map(c => c.UeNome).ToColumn("ue_nome");
            Map(c => c.QuantidadeTurmas).ToColumn("quantidade_turmas");
            Map(c => c.QuantidadeEstudantes).ToColumn("quantidade_estudantes");
            Map(c => c.QuantidadeEstudantesComFrequenciaInferiorLimite).ToColumn("quantidade_estudantes_com_frequencia_inferior_limite");
            Map(c => c.QuantidadeEstudantesDificuldadeTop1).ToColumn("quantidade_estudantes_dificuldade_top_1");
            Map(c => c.QuantidadeEstudantesDificuldadeTop2).ToColumn("quantidade_estudantes_dificuldade_top_2");
            Map(c => c.OutrasDificuldadesAprendizagem).ToColumn("outras_dificuldades_aprendizagem");
            Map(c => c.NomeDificuldadeTop1).ToColumn("nome_dificuldade_top_1");
            Map(c => c.NomeDificuldadeTop2).ToColumn("nome_dificuldade_top_2");
        }
    }
}
