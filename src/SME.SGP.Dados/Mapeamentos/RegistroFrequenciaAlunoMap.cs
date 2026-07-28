using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class RegistroFrequenciaAlunoMap : BaseEntityMap<RegistroFrequenciaAluno>
    {
        public RegistroFrequenciaAlunoMap()
        {
            ToTable("registro_frequencia_aluno");
            Map(nameof(RegistroFrequenciaAluno.CodigoAluno), "codigo_aluno");
            Map(nameof(RegistroFrequenciaAluno.NumeroAula), "numero_aula");
            Map(nameof(RegistroFrequenciaAluno.Valor), "valor");
            Map(nameof(RegistroFrequenciaAluno.RegistroFrequenciaId), "registro_frequencia_id");
            Map(nameof(RegistroFrequenciaAluno.AulaId), "aula_id");
            Map(nameof(RegistroFrequenciaAluno.Excluido), "excluido");
        }
    }
}