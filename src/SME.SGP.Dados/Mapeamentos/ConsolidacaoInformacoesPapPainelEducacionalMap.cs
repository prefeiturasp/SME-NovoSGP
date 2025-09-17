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
            Map(c => c.QuantidadeTurmas).ToColumn("quantidade_turmas");
            Map(c => c.QuantidadeEstudantes).ToColumn("quantidade_estudantes");
            Map(c => c.QuantidadeEstudantesComMenosDe75PorcentoFrequencia).ToColumn("quantidade_estudantes_com_menos_75_por_cento_frequencia");
            Map(c => c.DificuldadeAprendizagem1).ToColumn("dificuldade_aprendizagem_1");
            Map(c => c.DificuldadeAprendizagem2).ToColumn("dificuldade_aprendizagem_2");
            Map(c => c.OutrasDificuldadesAprendizagem).ToColumn("outras_dificuldades_aprendizagem");
        }
    }
}
