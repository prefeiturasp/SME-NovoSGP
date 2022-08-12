using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class PlanoAEEMap : BaseMap<PlanoAEE>
    {
        public PlanoAEEMap()
        {
            ToTable("plano_aee");
            Map(c => c.TurmaId).ToColumn("turma_id");
            Map(c => c.AlunoNumero).ToColumn("aluno_numero");
            Map(c => c.AlunoCodigo).ToColumn("aluno_codigo");
            Map(c => c.AlunoNome).ToColumn("aluno_nome");
            Map(c => c.Situacao).ToColumn("situacao");
            Map(c => c.ParecerCoordenacao).ToColumn("parecer_coordenacao");
            Map(c => c.ParecerPAAI).ToColumn("parecer_paai");
            Map(c => c.ResponsavelId).ToColumn("responsavel_id");
        }
    }
}
