using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class NotificacaoAulaPrevistaMap : BaseMap<NotificacaoAulaPrevista>
    {
        public NotificacaoAulaPrevistaMap()
        {
            ToTable("notificacao_aula_prevista");
            Map(nameof(NotificacaoAulaPrevista.NotificacaoCodigo), "notificacao_id");
            Map(nameof(NotificacaoAulaPrevista.DisciplinaId), "disciplina_id");
            Map(nameof(NotificacaoAulaPrevista.TurmaId), "turma_id");
            Map(nameof(NotificacaoAulaPrevista.Bimestre), "bimestre");
            Map(nameof(NotificacaoAulaPrevista.Excluido), "excluido");
        }
    }
}