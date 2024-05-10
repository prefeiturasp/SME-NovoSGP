using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConselhoClasseParecerAnoMap : BaseMap<ConselhoClasseParecerAno>
    {
        public ConselhoClasseParecerAnoMap()
        {
            ToTable("conselho_classe_parecer_ano");
            Map(c => c.ParecerId).ToColumn("parecer_id");
            Map(c => c.AnoTurma).ToColumn("ano_turma");
            Map(c => c.Modalidade).ToColumn("modalidade");
            Map(c => c.InicioVigencia).ToColumn("inicio_vigencia");
            Map(c => c.FimVigencia).ToColumn("fim_vigencia");
        }
    }
}
