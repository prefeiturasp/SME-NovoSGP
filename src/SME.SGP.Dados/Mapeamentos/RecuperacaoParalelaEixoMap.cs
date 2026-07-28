using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class RecuperacaoParalelaEixoMap : BaseEntityMap<RecuperacaoParalelaEixo>
    {
        public RecuperacaoParalelaEixoMap()
        {
            ToTable("recuperacao_paralela_eixo");
            Map(nameof(RecuperacaoParalelaEixo.Descricao), "descricao");
            Map(nameof(RecuperacaoParalelaEixo.DtFim), "dt_fim");
            Map(nameof(RecuperacaoParalelaEixo.DtInicio), "dt_inicio");
            Map(nameof(RecuperacaoParalelaEixo.Excluido), "excluido");
            Map(nameof(RecuperacaoParalelaEixo.RecuperacaoParalelaPeriodoId), "recuperacao_paralela_periodo_id");
        }
    }
}