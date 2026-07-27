using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class AulaPrevistaMap : BaseEntityMap<AulaPrevista>
    {
        public AulaPrevistaMap()
        {
            ToTable("aula_prevista");
            Map(nameof(AulaPrevista.TipoCalendarioId), "tipo_calendario_id");
            Map(nameof(AulaPrevista.DisciplinaId), "disciplina_id");
            Map(nameof(AulaPrevista.TurmaId), "turma_id");
        }
    }
}