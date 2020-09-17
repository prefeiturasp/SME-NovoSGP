using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class RecuperacaoParalelaEixoMap : BaseMap<RecuperacaoParalelaEixo>
    {
        public RecuperacaoParalelaEixoMap()
        {
            ToTable("recuperacao_paralela_eixo");
            Map(c => c.DtInicio).ToColumn("dt_inicio");
            Map(c => c.DtFim).ToColumn("dt_fim");
            Map(c => c.RecuperacaoParalelaPeriodoId).ToColumn("recuperacao_paralela_periodo_id");
        }
    }
}