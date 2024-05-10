using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class RegistroAusenciaAlunoMap : BaseMap<RegistroAusenciaAluno>
    {
        public RegistroAusenciaAlunoMap()
        {
            ToTable("registro_ausencia_aluno");
            Map(c => c.CodigoAluno).ToColumn("codigo_aluno");
            Map(c => c.Migrado).ToColumn("migrado");
            Map(c => c.NumeroAula).ToColumn("numero_aula");
            Map(c => c.RegistroFrequenciaId).ToColumn("registro_frequencia_id");
        }
    }
}