using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class AnotacaoFrequenciaAlunoMap : BaseEntityMap<AnotacaoFrequenciaAluno>
    {
        public AnotacaoFrequenciaAlunoMap()
        {
            ToTable("anotacao_frequencia_aluno");
            Map(nameof(AnotacaoFrequenciaAluno.MotivoAusenciaId), "motivo_ausencia_id");
            Map(nameof(AnotacaoFrequenciaAluno.AulaId), "aula_id");
            Map(nameof(AnotacaoFrequenciaAluno.Anotacao), "anotacao");
            Map(nameof(AnotacaoFrequenciaAluno.CodigoAluno), "codigo_aluno");
            Map(nameof(AnotacaoFrequenciaAluno.Excluido), "excluido");
        }
    }
}