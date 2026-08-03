using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class FechamentoNotaMap : BaseMap<FechamentoNota>
    {
        public FechamentoNotaMap()
        {
            ToTable("fechamento_nota");
            Map(nameof(FechamentoNota.SinteseId), "sintese_id");
            Map(nameof(FechamentoNota.FechamentoAlunoId), "fechamento_aluno_id");
            Map(nameof(FechamentoNota.DisciplinaId), "disciplina_id");
            Map(nameof(FechamentoNota.Nota), "nota");
            Map(nameof(FechamentoNota.ConceitoId), "conceito_id");
            Map(nameof(FechamentoNota.Migrado), "migrado");
            Map(nameof(FechamentoNota.Excluido), "excluido");
        }
    }
}