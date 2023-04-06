using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public  class CompensacaoAusenciaAlunoAulaMap : BaseMap<CompensacaoAusenciaAlunoAula>
    {
        public CompensacaoAusenciaAlunoAulaMap()
        {
            ToTable("compensacao_ausencia_aluno_aula");
            Map(c => c.Excluido).ToColumn("excluido");
            Map(c => c.CompensacaoAusenciaAlunoId).ToColumn("compensacao_ausencia_aluno_id");
            Map(c => c.RegistroFrequenciaAlunoId).ToColumn("registro_frequencia_aluno_id");
            Map(c => c.NumeroAula).ToColumn("numero_aula");
            Map(c => c.DataAula).ToColumn("data_aula");
        }
    }
}
