using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConselhoClasseNotaMap : BaseEntityMap<ConselhoClasseNota>
    {
        public ConselhoClasseNotaMap()
        {
            ToTable("conselho_classe_nota");
            Map(nameof(ConselhoClasseNota.ConselhoClasseAlunoId), "conselho_classe_aluno_id");
            Map(nameof(ConselhoClasseNota.ComponenteCurricularCodigo), "componente_curricular_codigo");
            Map(nameof(ConselhoClasseNota.Nota), "nota");
            Map(nameof(ConselhoClasseNota.ConceitoId), "conceito_id");
            Map(nameof(ConselhoClasseNota.Justificativa), "justificativa");
            Map(nameof(ConselhoClasseNota.Excluido), "excluido");
            Map(nameof(ConselhoClasseNota.Migrado), "migrado");
        }
    }
}