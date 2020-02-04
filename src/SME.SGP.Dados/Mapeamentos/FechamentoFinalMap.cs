using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class FechamentoFinalMap : BaseMap<FechamentoFinal>
    {
        public FechamentoFinalMap()
        {
            ToTable("fechamento_final");
            Map(a => a.AusenciasCompensadas).ToColumn("ausencias_compensadas");
            Map(a => a.AlunoCodigo).ToColumn("aluno_codigo");
            Map(a => a.DisciplinaCodigo).ToColumn("disciplina_codigo");
            Map(a => a.PercentualFrequencia).ToColumn("percentual_frequencia");

            Map(a => a.ConceitoId).ToColumn("conceito_id");
            Map(a => a.Conceito).Ignore();

            Map(a => a.TurmaId).ToColumn("turma_id");
            Map(a => a.Turma).Ignore();
        }
    }
}