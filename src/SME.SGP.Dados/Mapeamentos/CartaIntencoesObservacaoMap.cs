using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class CartaIntencoesObservacaoMap : BaseEntityMap<CartaIntencoesObservacao>
    {
        public CartaIntencoesObservacaoMap()
        {
            ToTable("carta_intencoes_observacao");
            Map(nameof(CartaIntencoesObservacao.Observacao), "observacao");
            Map(nameof(CartaIntencoesObservacao.TurmaId), "turma_id");
            Map(nameof(CartaIntencoesObservacao.ComponenteCurricularId), "componente_curricular_id");
            Map(nameof(CartaIntencoesObservacao.UsuarioId), "usuario_id");
            Map(nameof(CartaIntencoesObservacao.Excluido), "excluido");
        }
    }
}