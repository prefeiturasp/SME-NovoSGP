using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class TipoAvaliacaoMap : BaseMap<TipoAvaliacao>
    {
        public TipoAvaliacaoMap()
        {
            ToTable("tipo_avaliacao");
            Map(c => c.AvaliacoesNecessariasPorBimestre).ToColumn("avaliacoes_necessarias_bimestre");
            Map(c => c.Descricao).ToColumn("descricao");
            Map(c => c.Excluido).ToColumn("excluido");
            Map(c => c.Nome).ToColumn("nome");
            Map(c => c.Situacao).ToColumn("situacao");
            Map(c => c.Codigo).ToColumn("codigo");
        }
    }
}