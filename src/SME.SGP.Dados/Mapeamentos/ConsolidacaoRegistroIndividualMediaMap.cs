using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados
{
    class ConsolidacaoRegistroIndividualMediaMap : DommelEntityMap<ConsolidacaoRegistroIndividualMedia>
    {
        public ConsolidacaoRegistroIndividualMediaMap()
        {
            ToTable("consolidacao_regsitro_individual_media");
            Map(c => c.TurmaId).ToColumn("turma_id");
            Map(c => c.Quantidade).ToColumn("quantidade");
        }
    }
}
