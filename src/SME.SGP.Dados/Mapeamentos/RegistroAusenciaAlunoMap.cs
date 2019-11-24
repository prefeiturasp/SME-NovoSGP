using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class RegistroAusenciaAlunoMap : BaseMap<RegistroAusenciaAluno>
    {
        public RegistroAusenciaAlunoMap()
        {
            ToTable("registro_ausencia_aluno");
            Map(e => e.CodigoAluno).ToColumn("codigo_aluno");
            Map(e => e.NumeroAula).ToColumn("numero_aula");
            Map(e => e.RegistroFrequenciaId).ToColumn("registro_frequencia_id");
        }
    }
}