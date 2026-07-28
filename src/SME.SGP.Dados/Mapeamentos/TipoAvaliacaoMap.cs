using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class TipoAvaliacaoMap : BaseEntityMap<TipoAvaliacao>
    {
        public TipoAvaliacaoMap()
        {
            ToTable("tipo_avaliacao");

            Map(nameof(TipoAvaliacao.AvaliacoesNecessariasPorBimestre), "avaliacoes_necessarias_bimestre");
            Map(nameof(TipoAvaliacao.Descricao), "descricao");
            Map(nameof(TipoAvaliacao.Excluido), "excluido");
            Map(nameof(TipoAvaliacao.Nome), "nome");
            Map(nameof(TipoAvaliacao.Situacao), "situacao");
            Map(nameof(TipoAvaliacao.Codigo), "codigo");
        }
    }
}