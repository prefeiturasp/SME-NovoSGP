using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class NotaConceitoMap : BaseMap<NotaConceito>
    {
        public NotaConceitoMap()
        {
            ToTable("notas_conceito");
            Map(n => n.AtividadeAvaliativaID).ToColumn("atividade_avaliativa");
            Map(n => n.AlunoId).ToColumn("aluno_id");
            Map(n => n.TipoNota).ToColumn("tipo_nota");
            Map(n => n.DisciplinaId).ToColumn("disciplina_id");
            Map(n => n.ConceitoId).ToColumn("conceito");
            Map(n => n.StatusGsa).ToColumn("status_gsa");
        }
    }
}