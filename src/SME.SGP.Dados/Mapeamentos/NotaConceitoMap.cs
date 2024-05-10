using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class NotaConceitoMap : BaseMap<NotaConceito>
    {
        public NotaConceitoMap()
        {
            ToTable("notas_conceito");
            Map(c => c.AlunoId).ToColumn("aluno_id");
            Map(c => c.AtividadeAvaliativaID).ToColumn("atividade_avaliativa");
            Map(c => c.ConceitoId).ToColumn("conceito");
            Map(c => c.DisciplinaId).ToColumn("disciplina_id");
            Map(c => c.Nota).ToColumn("nota");
            Map(c => c.TipoNota).ToColumn("tipo_nota");
            Map(c => c.StatusGsa).ToColumn("status_gsa");
        }
    }
}