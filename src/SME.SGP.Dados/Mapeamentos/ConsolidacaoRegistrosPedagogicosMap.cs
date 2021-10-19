using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConsolidacaoRegistrosPedagogicosMap : DommelEntityMap<ConsolidacaoRegistrosPedagogicos>
    {
        public ConsolidacaoRegistrosPedagogicosMap()
        {
            ToTable("consolidacao_registros_pedagogicos");
            Map(a => a.TurmaId).ToColumn("turma_id");
            Map(a => a.AnoLetivo).ToColumn("ano_letivo");
            Map(a => a.ComponenteId).ToColumn("componente_curricular_id");
            Map(a => a.NomeProfessor).ToColumn("nome_professor");
            Map(a => a.RFProfessor).ToColumn("rf_professor");
            Map(a => a.FrequenciasPendentes).ToColumn("frequencias_pendentes");
            Map(a => a.DataUltimaFrequencia).ToColumn("data_ultima_frequencia");
            Map(a => a.DataUltimoDiarioBordo).ToColumn("data_ultimo_diariobordo");
            Map(a => a.DataUltimoPlanoAula).ToColumn("data_ultimo_planoaula");
            Map(a => a.DiarioBordoPendentes).ToColumn("diario_bordo_pendentes");
            Map(a => a.PlanoAulaPendentes).ToColumn("planos_aula_pendentes");
            Map(a => a.PeriodoEscolarId).ToColumn("periodo_escolar_id");
            Map(a => a.QuantidadeAulas).ToColumn("quantidade_aulas");
        }
    }
}
