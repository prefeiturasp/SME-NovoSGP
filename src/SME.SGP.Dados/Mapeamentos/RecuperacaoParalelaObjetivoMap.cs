using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class RecuperacaoParalelaObjetivoMap : BaseMap<RecuperacaoParalelaObjetivo>
    {
        public RecuperacaoParalelaObjetivoMap()
        {
            ToTable("recuperacao_paralela_objetivo");
            Map(nameof(RecuperacaoParalelaObjetivo.Descricao), "descricao");
            Map(nameof(RecuperacaoParalelaObjetivo.DtFim), "dt_fim");
            Map(nameof(RecuperacaoParalelaObjetivo.DtInicio), "dt_inicio");
            Map(nameof(RecuperacaoParalelaObjetivo.EhEspecifico), "eh_especifico");
            Map(nameof(RecuperacaoParalelaObjetivo.EixoId), "eixo_id");
            Map(nameof(RecuperacaoParalelaObjetivo.Excluido), "excluido");
            Map(nameof(RecuperacaoParalelaObjetivo.Nome), "nome");
            Map(nameof(RecuperacaoParalelaObjetivo.Ordem), "ordem");
        }
    }
}