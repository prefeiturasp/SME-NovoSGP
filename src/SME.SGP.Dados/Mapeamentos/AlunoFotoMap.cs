using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class AlunoFotoMap : BaseMap<AlunoFoto>
    {
        public AlunoFotoMap()
        {
            ToTable("aluno_foto");
            Map(a => a.MiniaturaId).ToColumn("miniatura_id");
            Map(a => a.ArquivoId).ToColumn("arquivo_id");
            Map(a => a.AlunoCodigo).ToColumn("aluno_codigo");
        }
    }
}
