﻿using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class WFAprovacaoParecerConclusivoMap : BaseMap<WFAprovacaoParecerConclusivo>
    {
        public WFAprovacaoParecerConclusivoMap()
        {
            ToTable("wf_aprovacao_parecer_conclusivo");
            Map(c => c.WfAprovacaoId).ToColumn("wf_aprovacao_id");
            Map(c => c.ConselhoClasseAlunoId).ToColumn("conselho_classe_aluno_id");
            Map(c => c.UsuarioSolicitanteId).ToColumn("usuario_solicitante_id");
            Map(c => c.ConselhoClasseParecerId).ToColumn("conselho_classe_parecer_id");
        }
    }
}
