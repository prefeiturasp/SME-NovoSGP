using SME.SGP.Dominio.Entidades;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PainelEducacionalVisaoGeralMap : BaseMap<PainelEducacionalVisaoGeral>
    {
        public PainelEducacionalVisaoGeralMap()
        {
            ToTable("painel_educacional_visao_geral");
            Map(nameof(PainelEducacionalVisaoGeral.CodigoDre), "codigo_dre");
            Map(nameof(PainelEducacionalVisaoGeral.CodigoUe), "codigo_ue");
            Map(nameof(PainelEducacionalVisaoGeral.AnoLetivo), "ano_letivo");
            Map(nameof(PainelEducacionalVisaoGeral.Indicador), "indicador");
            Map(nameof(PainelEducacionalVisaoGeral.Serie), "serie");
            Map(nameof(PainelEducacionalVisaoGeral.Valor), "valor");
        }
    }
}