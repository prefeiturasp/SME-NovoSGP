using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ItineranciaAlunoMap : BaseMap<ItineranciaAluno>
    {
        public ItineranciaAlunoMap()
        {
            ToTable("itinerancia_aluno");
            Map(nameof(ItineranciaAluno.CodigoAluno), "codigo_aluno");
            Map(nameof(ItineranciaAluno.ItineranciaId), "itinerancia_id");
            Map(nameof(ItineranciaAluno.TurmaId), "turma_id");
            Map(nameof(ItineranciaAluno.Excluido), "excluido");
        }
    }
}