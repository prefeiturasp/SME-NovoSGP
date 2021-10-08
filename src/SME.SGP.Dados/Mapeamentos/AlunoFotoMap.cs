using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class AlunoFotoMap : BaseMap<AlunoFoto>
    {
        public AlunoFotoMap()
        {
            ToTable("aluno_foto");
            Map(c => c.ArquivoId).ToColumn("arquivo_id");
            Map(c => c.MiniaturaId).ToColumn("miniatura_id");
            Map(c => c.AlunoCodigo).ToColumn("aluno_codigo");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}
