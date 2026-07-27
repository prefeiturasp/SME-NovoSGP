using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class CompensacaoAusenciaAlunoAulaMap : BaseEntityMap<CompensacaoAusenciaAlunoAula>
    {
        public CompensacaoAusenciaAlunoAulaMap()
        {
            ToTable("compensacao_ausencia_aluno_aula");
            Map(nameof(CompensacaoAusenciaAlunoAula.Excluido), "excluido");
            Map(nameof(CompensacaoAusenciaAlunoAula.CompensacaoAusenciaAlunoId), "compensacao_ausencia_aluno_id");
            Map(nameof(CompensacaoAusenciaAlunoAula.RegistroFrequenciaAlunoId), "registro_frequencia_aluno_id");
            Map(nameof(CompensacaoAusenciaAlunoAula.NumeroAula), "numero_aula");
            Map(nameof(CompensacaoAusenciaAlunoAula.DataAula), "data_aula");
        }
    }
}