using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class AcompanhamentoTurmaMap : BaseMap<AcompanhamentoTurma>
    {
        public AcompanhamentoTurmaMap()
        {
            ToTable("acompanhamento_turma");
            Map(c => c.TurmaId).ToColumn("turma_id");
            Map(c => c.Semestre).ToColumn("semestre");
            Map(c => c.ApanhadoGeral).ToColumn("apanhado_geral");
        }
    }
}
