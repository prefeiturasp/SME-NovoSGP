using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    class ConsolidacaoRegistroIndividualMediaMap : DommelEntityMap<ConsolidacaoRegistroIndividualMedia>
    {
        public ConsolidacaoRegistroIndividualMediaMap()
        {
            ToTable("consolidacao_registro_individual_media");
            Map(c => c.TurmaId).ToColumn("turma_id");
            Map(c => c.Quantidade).ToColumn("quantidade");
        }
    }
}
