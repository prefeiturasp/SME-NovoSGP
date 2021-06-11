using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class RegistroFrequenciaAlunoMap : BaseMap<RegistroFrequenciaAluno>
    {
        public RegistroFrequenciaAlunoMap()
        {
            ToTable("registro_frequencia_aluno");
            Map(a => a.Id).ToColumn("id");
            Map(a => a.CodigoAluno).ToColumn("codigo_aluno");
            Map(a => a.NumeroAula).ToColumn("numero_aula");
            Map(a => a.RegistroFrequenciaId).ToColumn("registro_frequencia_id");
            Map(a => a.Valor).ToColumn("valor");
        }
    }
}
