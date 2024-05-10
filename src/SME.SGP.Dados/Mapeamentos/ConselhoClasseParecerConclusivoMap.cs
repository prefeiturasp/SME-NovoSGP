using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConselhoClasseParecerConclusivoMap : BaseMap<ConselhoClasseParecerConclusivo>
    {
        public ConselhoClasseParecerConclusivoMap()
        {
            ToTable("conselho_classe_parecer");
            Map(c => c.Nome).ToColumn("nome");
            Map(c => c.Aprovado).ToColumn("aprovado");
            Map(c => c.Frequencia).ToColumn("frequencia");
            Map(c => c.Nota).ToColumn("nota");
            Map(c => c.Conselho).ToColumn("conselho");
            Map(c => c.InicioVigencia).ToColumn("inicio_vigencia");
            Map(c => c.FimVigencia).ToColumn("fim_vigencia");
        }
    }
}
