﻿using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConselhoClasseAlunoMap: BaseMap<ConselhoClasseAluno>
    {
        public ConselhoClasseAlunoMap()
        {
            ToTable("conselho_classe_aluno");
            Map(c => c.ConselhoClasseId).ToColumn("conselho_classe_id");
            Map(c => c.AlunoCodigo).ToColumn("aluno_codigo");
            Map(c => c.RecomendacoesAluno).ToColumn("recomendacoes_aluno");
            Map(c => c.RecomendacoesFamilia).ToColumn("recomendacoes_familia");
            Map(c => c.AnotacoesPedagogicas).ToColumn("anotacoes_pedagogicas");
            Map(c => c.ConselhoClasseParecerId).ToColumn("conselho_classe_parecer_id");
            Map(c => c.Excluido).ToColumn("excluido");
            Map(c => c.Migrado).ToColumn("migrado");
        }
    }
}
