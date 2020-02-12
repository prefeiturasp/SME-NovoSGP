using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class EixoMap : BaseMap<Eixo>
    {
        public EixoMap()
        {
            ToTable("eixo");
            Map(c => c.DtInicio).ToColumn("dt_inicio");
            Map(c => c.DtFim).ToColumn("dt_fim");
            Map(c => c.RecuperacaoParalelaPeriodoId).ToColumn("recuperacao_paralela_periodo_id");
        }
    }
}