using SME.SGP.Dominio.Entidades;

namespace SME.SGP.Dados.Mapeamentos
{
    public class NotaConceitoCicloParametroMap : BaseMap<NotaConceitoCicloParametro>
    {
        public NotaConceitoCicloParametroMap()
        {
            ToTable("notas_conceitos_ciclos_parametos");
            Map(nameof(NotaConceitoCicloParametro.Ativo), "ativo");
            Map(nameof(NotaConceitoCicloParametro.CicloId), "ciclo");
            Map(nameof(NotaConceitoCicloParametro.FimVigencia), "fim_vigencia");
            Map(nameof(NotaConceitoCicloParametro.InicioVigencia), "inicio_vigencia");
            Map(nameof(NotaConceitoCicloParametro.PercentualAlerta), "percentual_alerta");
            Map(nameof(NotaConceitoCicloParametro.QtdMinimaAvalicoes), "qtd_minima_avaliacao");
            Map(nameof(NotaConceitoCicloParametro.TipoNotaId), "tipo_nota");
        }
    }
}