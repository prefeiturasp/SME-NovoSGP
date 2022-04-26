using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class ConselhoClasseAlunoRecomendacaoMap : DommelEntityMap<ConselhoClasseAlunoRecomendacao>
    {
        public ConselhoClasseAlunoRecomendacaoMap()
        {
            ToTable("conselho_classe_aluno_recomendacao");
            Map(c => c.Id).ToColumn("id").IsIdentity().IsKey(); 
            Map(c => c.ConselhoClasseAlunoId).ToColumn("conselho_classe_aluno_id");
            Map(c => c.ConselhoClasseRecomendacaoId).ToColumn("conselho_classe_recomendacao_id");
        }
    }
}
