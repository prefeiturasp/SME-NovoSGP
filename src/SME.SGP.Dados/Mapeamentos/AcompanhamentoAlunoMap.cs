using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class AcompanhamentoAlunoMap : BaseMap<AcompanhamentoAluno>
    {
        public AcompanhamentoAlunoMap()
        {
            ToTable("acompanhamento_aluno");
            Map(a => a.TurmaId).ToColumn("turma_id");
            Map(a => a.AlunoCodigo).ToColumn("aluno_codigo");
        }
    }
}
