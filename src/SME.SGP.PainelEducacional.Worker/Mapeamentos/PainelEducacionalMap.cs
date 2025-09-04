using Dapper.FluentMap.Dommel.Mapping;

namespace SME.SGP.PainelEducacional.Worker.Mapeamentos
{
    public class PainelEducacionalMap : DommelEntityMap<SME.SGP.Dominio.Entidades.PainelEducacionalConsolidacaoIdep>
    {
        public PainelEducacionalMap()
        {
            ToTable("painel_educacional_consolidacao_idep");
            Map(c => c.Id).ToColumn("id").IsKey();
            Map(c => c.AnoLetivo).ToColumn("ano_letivo");
            Map(c => c.Etapa).ToColumn("etapa");
            Map(c => c.Faixa).ToColumn("faixa");
            Map(c => c.Quantidade).ToColumn("quantidade");
            Map(c => c.MediaGeral).ToColumn("media_geral");
            Map(c => c.UltimaAtualizacao).ToColumn("ultima_atualizacao");
        }
    }
}
