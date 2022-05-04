using SME.SGP.Dominio.Entidades;

namespace SME.SGP.Dados.Mapeamentos
{
    public class NotaConceitoCicloParametroMap : BaseMap<NotaConceitoCicloParametro>
    {
        public NotaConceitoCicloParametroMap()
        {
            ToTable("notas_conceitos_ciclos_parametos");
            Map(c => c.Ativo).ToColumn("ativo");
            Map(c => c.CicloId).ToColumn("ciclo");
            Map(c => c.FimVigencia).ToColumn("fim_vigencia");
            Map(c => c.InicioVigencia).ToColumn("inicio_vigencia");
            Map(c => c.PercentualAlerta).ToColumn("percentual_alerta");
            Map(c => c.QtdMinimaAvalicoes).ToColumn("qtd_minima_avaliacao");
            Map(c => c.TipoNotaId).ToColumn("tipo_nota");
        }
    }
}