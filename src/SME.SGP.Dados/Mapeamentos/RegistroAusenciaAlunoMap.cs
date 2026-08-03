using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class RegistroAusenciaAlunoMap : BaseMap<RegistroAusenciaAluno>
    {
        public RegistroAusenciaAlunoMap()
        {
            ToTable("registro_ausencia_aluno");
            Map(nameof(RegistroAusenciaAluno.CodigoAluno), "codigo_aluno");
            Map(nameof(RegistroAusenciaAluno.Migrado), "migrado");
            Map(nameof(RegistroAusenciaAluno.NumeroAula), "numero_aula");
            Map(nameof(RegistroAusenciaAluno.RegistroFrequenciaId), "registro_frequencia_id");
        }
    }
}