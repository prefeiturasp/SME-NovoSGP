using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConsolidacaoFrequenciaAlunoMensalMap : DommelEntityMap<ConsolidacaoFrequenciaAlunoMensal>
    {
        public ConsolidacaoFrequenciaAlunoMensalMap()
        {
            ToTable("consolidacao_frequencia_aluno_mensal");
            Map(c => c.Id).ToColumn("id").IsIdentity().IsKey();
            Map(c => c.TurmaId).ToColumn("turma_id");
            Map(c => c.AlunoCodigo).ToColumn("aluno_codigo");
            Map(c => c.Mes).ToColumn("mes");
            Map(c => c.Percentual).ToColumn("percentual");
            Map(c => c.QuantidadeAulas).ToColumn("quantidade_aulas");
            Map(c => c.QuantidadeAusencias).ToColumn("quantidade_ausencias");
            Map(c => c.QuantidadeCompensacoes).ToColumn("quantidade_compensacoes");
        }
    }
}
