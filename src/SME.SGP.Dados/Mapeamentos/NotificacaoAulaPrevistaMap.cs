using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class NotificacaoAulaPrevistaMap : BaseMap<NotificacaoAulaPrevista>
    {
        public NotificacaoAulaPrevistaMap()
        {
            ToTable("notificacao_aula_prevista");
            Map(c => c.NotificacaoCodigo).ToColumn("notificacao_id");
            Map(c => c.DisciplinaId).ToColumn("disciplina_id");
            Map(c => c.TurmaId).ToColumn("turma_id");
            Map(c => c.Bimestre).ToColumn("bimestre");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}
