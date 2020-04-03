using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class NotaConceitoBimestreMap : BaseMap<NotaConceitoBimestre>
    {
        public NotaConceitoBimestreMap()
        {
            ToTable("nota_conceito_bimestre");
            Map(c => c.FechamentoTurmaDisciplinaId).ToColumn("fechamento_turma_disciplina_id");
            Map(c => c.DisciplinaId).ToColumn("disciplina_id");
            Map(c => c.CodigoAluno).ToColumn("codigo_aluno");
            Map(c => c.ConceitoId).ToColumn("conceito_id");
            Map(c => c.SinteseId).ToColumn("sintese_id");
        }
    }
}