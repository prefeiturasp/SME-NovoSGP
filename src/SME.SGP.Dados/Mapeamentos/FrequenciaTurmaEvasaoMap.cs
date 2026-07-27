using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class FrequenciaTurmaEvasaoMap : SimpleEntityMap<FrequenciaTurmaEvasao>
    {
        public FrequenciaTurmaEvasaoMap()
        {
            ToTable("frequencia_turma_evasao");
            Map(nameof(FrequenciaTurmaEvasao.TurmaId), "turma_id");
            Map(nameof(FrequenciaTurmaEvasao.Mes), "mes");
            Map(nameof(FrequenciaTurmaEvasao.QuantidadeAlunosAbaixo50Porcento), "quantidade_alunos_abaixo_50_porcento");
            Map(nameof(FrequenciaTurmaEvasao.QuantidadeAlunos0Porcento), "quantidade_alunos_0_porcento");
        }
    }
}