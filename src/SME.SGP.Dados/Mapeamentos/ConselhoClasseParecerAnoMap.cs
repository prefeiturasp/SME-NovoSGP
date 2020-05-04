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
            Map(x => x.ParecerId).ToColumn("parecer_id");
            Map(x => x.AnoTurma).ToColumn("ano_turma");
            Map(x => x.Modalidade).ToColumn("modalidade");
            Map(x => x.InicioVigencia).ToColumn("inicio_vigencia");
            Map(x => x.FimVigencia).ToColumn("fim_vigencia");
        }
    }
}
