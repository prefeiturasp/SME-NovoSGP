using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class NotaConceitoMap : BaseMap<NotaConceito>
    {
        public NotaConceitoMap()
        {
            ToTable("notas_conceito");
            Map(nameof(NotaConceito.AlunoId), "aluno_id");
            Map(nameof(NotaConceito.AtividadeAvaliativaID), "atividade_avaliativa");
            Map(nameof(NotaConceito.ConceitoId), "conceito");
            Map(nameof(NotaConceito.DisciplinaId), "disciplina_id");
            Map(nameof(NotaConceito.Nota), "nota");
            Map(nameof(NotaConceito.TipoNota), "tipo_nota");
            Map(nameof(NotaConceito.StatusGsa), "status_gsa");
        }
    }
}