using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ItineranciaAlunoMap : BaseMap<ItineranciaAluno>
    {
        public ItineranciaAlunoMap()
        {
            ToTable("itinerancia_aluno");
            Map(c => c.CodigoAluno).ToColumn("codigo_aluno");
            Map(c => c.ItineranciaId).ToColumn("itinerancia_id");
            Map(c => c.TurmaId).ToColumn("turma_id");
        }
    }
}
