using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class TipoAvaliacaoMap : BaseMap<TipoAvaliacao>
    {
        public TipoAvaliacaoMap()
        {
            ToTable("tipo_avaliacao");
            Map(t => t.Nome).ToColumn("nome");
            Map(t => t.Descricao).ToColumn("descricao");
            Map(t => t.Excluido).ToColumn("excluido");
            Map(t => t.Situacao).ToColumn("situacao");
            Map(t => t.AvaliacoesNecessariasPorBimestre).ToColumn("avaliacoes_necessarias_bimestre");
        }
    }
}