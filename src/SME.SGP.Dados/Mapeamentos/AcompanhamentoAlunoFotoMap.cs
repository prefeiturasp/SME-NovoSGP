using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class AcompanhamentoAlunoFotoMap : BaseMap<AcompanhamentoAlunoFoto>
    {
        public AcompanhamentoAlunoFotoMap()
        {
            ToTable("acompanhamento_aluno_foto");
            Map(c => c.AcompanhamentoAlunoSemestreId).ToColumn("acompanhamento_aluno_semestre_id");
            Map(c => c.ArquivoId).ToColumn("arquivo_id");
            Map(c => c.MiniaturaId).ToColumn("miniatura_id");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}
