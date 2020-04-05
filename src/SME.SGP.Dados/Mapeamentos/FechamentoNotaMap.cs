using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class FechamentoNotaMap : BaseMap<FechamentoNota>
    {
        public FechamentoNotaMap()
        {
            ToTable("fechamento_nota");
            Map(c => c.FechamentoAlunoId).ToColumn("fechamento_aluno_id");
            Map(c => c.DisciplinaId).ToColumn("disciplina_id");
            Map(c => c.ConceitoId).ToColumn("conceito_id");
            Map(c => c.SinteseId).ToColumn("sintese_id");
        }
    }
}