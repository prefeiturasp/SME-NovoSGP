using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class AlunoFotoMap : BaseEntityMap<AlunoFoto>
    {
        public AlunoFotoMap()
        {
            ToTable("aluno_foto");
            Map(nameof(AlunoFoto.ArquivoId), "arquivo_id");
            Map(nameof(AlunoFoto.MiniaturaId), "miniatura_id");
            Map(nameof(AlunoFoto.AlunoCodigo), "aluno_codigo");
            Map(nameof(AlunoFoto.Excluido), "excluido");
        }
    }
}