using SME.SGP.Dados.Mapeamentos;
using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class ConsolidacaoRegistroIndividualMediaMap : SimpleMap<ConsolidacaoRegistroIndividualMedia>
    {
        public ConsolidacaoRegistroIndividualMediaMap()
        {
            ToTable("consolidacao_registro_individual_media");
            Map(nameof(ConsolidacaoRegistroIndividualMedia.TurmaId), "turma_id");
            Map(nameof(ConsolidacaoRegistroIndividualMedia.Quantidade), "quantidade");
        }
    }
}