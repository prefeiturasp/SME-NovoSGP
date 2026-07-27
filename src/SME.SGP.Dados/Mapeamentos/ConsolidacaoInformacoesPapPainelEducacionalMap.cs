using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConsolidacaoInformacoesPapMap : SimpleEntityMap<ConsolidacaoInformacoesPap>
    {
        public ConsolidacaoInformacoesPapMap()
        {
            ToTable("consolidacao_informacoes_pap");
            Map(nameof(ConsolidacaoInformacoesPap.TipoPap), "tipo_pap");
            Map(nameof(ConsolidacaoInformacoesPap.DreCodigo), "dre_codigo");
            Map(nameof(ConsolidacaoInformacoesPap.DreNome), "dre_nome");
            Map(nameof(ConsolidacaoInformacoesPap.UeCodigo), "ue_codigo");
            Map(nameof(ConsolidacaoInformacoesPap.UeNome), "ue_nome");
            Map(nameof(ConsolidacaoInformacoesPap.QuantidadeTurmas), "quantidade_turmas");
            Map(nameof(ConsolidacaoInformacoesPap.QuantidadeEstudantes), "quantidade_estudantes");
            Map(nameof(ConsolidacaoInformacoesPap.QuantidadeEstudantesComFrequenciaInferiorLimite), "quantidade_estudantes_com_frequencia_inferior_limite");
            Map(nameof(ConsolidacaoInformacoesPap.QuantidadeEstudantesDificuldadeTop1), "quantidade_estudantes_dificuldade_top_1");
            Map(nameof(ConsolidacaoInformacoesPap.QuantidadeEstudantesDificuldadeTop2), "quantidade_estudantes_dificuldade_top_2");
            Map(nameof(ConsolidacaoInformacoesPap.OutrasDificuldadesAprendizagem), "outras_dificuldades_aprendizagem");
            Map(nameof(ConsolidacaoInformacoesPap.NomeDificuldadeTop1), "nome_dificuldade_top_1");
            Map(nameof(ConsolidacaoInformacoesPap.NomeDificuldadeTop2), "nome_dificuldade_top_2");
        }
    }
}