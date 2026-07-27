using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConsolidacaoRegistrosPedagogicosMap : SimpleEntityMap<ConsolidacaoRegistrosPedagogicos>
    {
        public ConsolidacaoRegistrosPedagogicosMap()
        {
            ToTable("consolidacao_registros_pedagogicos");
            Map(nameof(ConsolidacaoRegistrosPedagogicos.TurmaId), "turma_id");
            Map(nameof(ConsolidacaoRegistrosPedagogicos.AnoLetivo), "ano_letivo");
            Map(nameof(ConsolidacaoRegistrosPedagogicos.ComponenteCurricularId), "componente_curricular_id");
            Map(nameof(ConsolidacaoRegistrosPedagogicos.NomeProfessor), "nome_professor");
            Map(nameof(ConsolidacaoRegistrosPedagogicos.RFProfessor), "rf_professor");
            Map(nameof(ConsolidacaoRegistrosPedagogicos.FrequenciasPendentes), "frequencias_pendentes");
            Map(nameof(ConsolidacaoRegistrosPedagogicos.DataUltimaFrequencia), "data_ultima_frequencia");
            Map(nameof(ConsolidacaoRegistrosPedagogicos.DataUltimoDiarioBordo), "data_ultimo_diariobordo");
            Map(nameof(ConsolidacaoRegistrosPedagogicos.DataUltimoPlanoAula), "data_ultimo_planoaula");
            Map(nameof(ConsolidacaoRegistrosPedagogicos.DiarioBordoPendentes), "diario_bordo_pendentes");
            Map(nameof(ConsolidacaoRegistrosPedagogicos.PlanoAulaPendentes), "planos_aula_pendentes");
            Map(nameof(ConsolidacaoRegistrosPedagogicos.PeriodoEscolarId), "periodo_escolar_id");
            Map(nameof(ConsolidacaoRegistrosPedagogicos.QuantidadeAulas), "quantidade_aulas");
            Map(nameof(ConsolidacaoRegistrosPedagogicos.CJ), "cj");
        }
    }
}