using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class CartaIntencoesObservacaoMap : BaseMap<CartaIntencoesObservacao>
    {
        public CartaIntencoesObservacaoMap()
        {
            ToTable("carta_intencoes_observacao");
            Map(c => c.CartaIntencoesId).ToColumn("carta_intencoes_id");
            Map(c => c.UsuarioId).ToColumn("usuario_id");
        }
    }
}