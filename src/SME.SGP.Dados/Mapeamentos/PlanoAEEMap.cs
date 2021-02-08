using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class PlanoAEEMap : BaseMap<PlanoAEE>
    {
        public PlanoAEEMap()
        {
            ToTable("plano_aee");
            Map(a => a.TurmaId).ToColumn("turma_id");
            Map(a => a.AlunoNumero).ToColumn("aluno_numero");
            Map(a => a.AlunoCodigo).ToColumn("aluno_codigo");
            Map(a => a.AlunoNome).ToColumn("aluno_nome");
            Map(a => a.Situacao).ToColumn("situacao");
        }
    }
}
