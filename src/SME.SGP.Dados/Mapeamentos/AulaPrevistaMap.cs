using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class AulaPrevistaMap : BaseMap<AulaPrevista>
    {
        public AulaPrevistaMap()
        {
            ToTable("aula_prevista");
            Map(c => c.TipoCalendarioId).ToColumn("tipo_calendario_id");
            Map(c => c.DisciplinaId).ToColumn("disciplina_id");
            Map(c => c.TurmaId).ToColumn("turma_id");
        }
    }
}
