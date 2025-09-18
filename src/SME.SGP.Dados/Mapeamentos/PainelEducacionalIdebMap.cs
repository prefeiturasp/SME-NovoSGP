using SME.SGP.Dominio.Entidades;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PainelEducacionalIdebMap : BaseMap<PainelEducacionalConsolidacaoIdeb>
    {
        public PainelEducacionalIdebMap()
        {
            ToTable("painel_educacional_consolidacao_ideb");
            Map(c => c.AnoLetivo).ToColumn("ano_letivo");
            Map(c => c.Etapa).ToColumn("etapa");
            Map(c => c.Faixa).ToColumn("faixa");
            Map(c => c.Quantidade).ToColumn("quantidade");
            Map(c => c.MediaGeral).ToColumn("media_geral");
            Map(c => c.UltimaAtualizacao).ToColumn("ultima_atualizacao");
            Map(c => c.CodigoDre).ToColumn("codigo_dre");
        }
    }
}
