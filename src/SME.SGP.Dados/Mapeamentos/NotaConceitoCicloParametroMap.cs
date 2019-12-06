using SME.SGP.Dominio.Entidades;

namespace SME.SGP.Dados.Mapeamentos
{
    public class NotaConceitoCicloParametroMap : BaseMap<NotaConceitoCicloParametro>
    {
        public NotaConceitoCicloParametroMap()
        {
            ToTable("notas_conceitos_ciclos_parametos");
            Map(n => n.InicioVigencia).ToColumn("inicio_vigencia");
            Map(n => n.TipoNotaId).ToColumn("tipo_nota");
            Map(n => n.QtdMinimaAvalicoes).ToColumn("qtd_minima_avaliacao");
            Map(n => n.PercentualAlerta).ToColumn("percentual_alerta");
            Map(n => n.FimVigencia).ToColumn("fim_vigencia");
        }
    }
}