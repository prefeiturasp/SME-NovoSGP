using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class CartaIntencoesMap : BaseEntityMap<CartaIntencoes>
    {
        public CartaIntencoesMap()
        {
            ToTable("carta_intencoes");
            Map(nameof(CartaIntencoes.TurmaId), "turma_id");
            Map(nameof(CartaIntencoes.PeriodoEscolarId), "periodo_escolar_id");
            Map(nameof(CartaIntencoes.ComponenteCurricularId), "componente_curricular_id");
            Map(nameof(CartaIntencoes.Planejamento), "planejamento");
            Map(nameof(CartaIntencoes.Excluido), "excluido");
        }
    }
}