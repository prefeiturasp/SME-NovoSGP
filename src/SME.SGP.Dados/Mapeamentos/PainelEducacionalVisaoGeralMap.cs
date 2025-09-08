using SME.SGP.Dominio.Entidades;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PainelEducacionalVisaoGeralMap : BaseMap<PainelEducacionalVisaoGeral>
    {
        public PainelEducacionalVisaoGeralMap()
        {
            ToTable("painel_educacional_visao_geral");
            Map(c => c.CodigoDre).ToColumn("codigo_dre");
            Map(c => c.AnoLetivo).ToColumn("ano_letivo");
            Map(c => c.Indicador).ToColumn("indicador");
            Map(c => c.Serie).ToColumn("serie");
            Map(c => c.Valor).ToColumn("valor");
        }
    }
}
