using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class FechamentoMap : BaseMap<Fechamento>
    {
        public FechamentoMap()
        {
            ToTable("fechamento");
            Map(c => c.DisciplinaId).ToColumn("disciplina_id");
            Map(c => c.PeriodoEscolarId).ToColumn("periodo_escolar_id");
            Map(c => c.TurmaId).ToColumn("turma_id");
            Map(c => c.Pendencias).Ignore();
            Map(c => c.PeriodoEscolar).Ignore();
            Map(c => c.Turma).Ignore();
        }
    }
}