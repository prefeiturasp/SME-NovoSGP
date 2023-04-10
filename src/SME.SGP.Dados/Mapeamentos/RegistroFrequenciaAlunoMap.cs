using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class RegistroFrequenciaAlunoMap : BaseMap<RegistroFrequenciaAluno>
    {
        public RegistroFrequenciaAlunoMap()
        {
            ToTable("registro_frequencia_aluno");
            Map(c => c.CodigoAluno).ToColumn("codigo_aluno");
            Map(c => c.NumeroAula).ToColumn("numero_aula");
            Map(c => c.Valor).ToColumn("valor");
            Map(c => c.RegistroFrequenciaId).ToColumn("registro_frequencia_id");
            Map(c => c.AulaId).ToColumn("aula_id");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}
