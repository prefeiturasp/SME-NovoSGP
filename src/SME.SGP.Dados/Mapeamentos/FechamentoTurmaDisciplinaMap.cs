using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class FechamentoTurmaDisciplinaMap: BaseMap<FechamentoTurmaDisciplina>
    {
        public FechamentoTurmaDisciplinaMap()
        {
            ToTable("fechamento_turma_disciplina");
            Map(c => c.FechamentoTurmaId).ToColumn("fechamento_turma_id");
            Map(c => c.DisciplinaId).ToColumn("disciplina_id");
        }
    }
}