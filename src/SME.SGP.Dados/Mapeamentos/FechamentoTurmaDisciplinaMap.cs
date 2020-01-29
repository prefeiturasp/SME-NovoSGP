using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class FechamentoTurmaDisciplinaMap: BaseMap<FechamentoTurmaDisciplina>
    {
        public FechamentoTurmaDisciplinaMap()
        {
            ToTable("fechamento_turma_disciplina");
            Map(c => c.FechamentoBimestreId).ToColumn("fechamento_bimestre_id");
            Map(c => c.TurmaId).ToColumn("turma_id");
            Map(c => c.DisciplinaId).ToColumn("disciplina_id");
        }
    }
}