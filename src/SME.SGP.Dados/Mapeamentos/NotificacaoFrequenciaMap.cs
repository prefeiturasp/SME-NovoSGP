using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class NotificacaoFrequenciaMap : BaseEntityMap<NotificacaoFrequencia>
    {
        public NotificacaoFrequenciaMap()
        {
            ToTable("notificacao_frequencia");
            Map(nameof(NotificacaoFrequencia.Tipo), "tipo");
            Map(nameof(NotificacaoFrequencia.NotificacaoCodigo), "notificacao_codigo");
            Map(nameof(NotificacaoFrequencia.DisciplinaCodigo), "disciplina_codigo");
            Map(nameof(NotificacaoFrequencia.AulaId), "aula_id");
            Map(nameof(NotificacaoFrequencia.Excluido), "excluido");
        }
    }
}