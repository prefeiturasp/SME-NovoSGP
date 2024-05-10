using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class CartaIntencoesObservacaoMap : BaseMap<CartaIntencoesObservacao>
    {
        public CartaIntencoesObservacaoMap()
        {
            ToTable("carta_intencoes_observacao");
            Map(c => c.Observacao).ToColumn("observacao");
            Map(c => c.TurmaId).ToColumn("turma_id");
            Map(c => c.ComponenteCurricularId).ToColumn("componente_curricular_id");
            Map(c => c.UsuarioId).ToColumn("usuario_id");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}