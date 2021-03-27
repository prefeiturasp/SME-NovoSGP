using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class AcompanhamentoTurmaMap : BaseMap<AcompanhamentoTurma>
    {
        public AcompanhamentoTurmaMap()
        {
            ToTable("acompanhamento_turma");
            Map(a => a.TurmaId).ToColumn("turma_id");
            Map(a => a.Semestre).ToColumn("semestre");
            Map(a => a.ApanhadoGeral).ToColumn("apanhado_geral");
        }
    }
}
