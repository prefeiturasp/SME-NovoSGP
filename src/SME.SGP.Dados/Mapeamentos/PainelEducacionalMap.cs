using SME.SGP.Dominio.Entidades;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PainelEducacionalMap : BaseMap<PainelEducacionalConsolidacaoIdep>
    {
        public PainelEducacionalMap()
        {
            ToTable("painel_educacional_consolidacao_idep");
            Map(c => c.AnoLetivo).ToColumn("ano_letivo");
            Map(c => c.Etapa).ToColumn("etapa");
            Map(c => c.Faixa).ToColumn("faixa");
            Map(c => c.Quantidade).ToColumn("quantidade");
            Map(c => c.MediaGeral).ToColumn("media_geral");
            Map(c => c.UltimaAtualizacao).ToColumn("ultima_atualizacao");
        }
    }
}
