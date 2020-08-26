using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class AnotacaoFrequenciaAlunoMap : BaseMap<AnotacaoFrequenciaAluno>
    {
        public AnotacaoFrequenciaAlunoMap()
        {
            ToTable("anotacao_frequencia_aluno");
            Map(c => c.MotivoAusenciaId).ToColumn("motivo_ausencia_id");
            Map(c => c.AulaId).ToColumn("aula_id");
            Map(c => c.CodigoAluno).ToColumn("codigo_aluno");
        }
    }
}
