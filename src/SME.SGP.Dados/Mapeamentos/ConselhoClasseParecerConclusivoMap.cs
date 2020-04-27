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
            Map(x => x.Nome).ToColumn("nome");
            Map(x => x.Aprovado).ToColumn("aprovado");
            Map(x => x.Frequencia).ToColumn("frequencia");
            Map(x => x.Conselho).ToColumn("conselho");
            Map(x => x.InicioVigencia).ToColumn("inicio_vigencia");
            Map(x => x.FimVigencia).ToColumn("fim_vigencia");
        }
    }
}
