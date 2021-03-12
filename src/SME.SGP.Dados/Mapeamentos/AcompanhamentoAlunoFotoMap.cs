using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class AcompanhamentoAlunoFotoMap : BaseMap<AcompanhamentoAlunoFoto>
    {
        public AcompanhamentoAlunoFotoMap()
        {
            ToTable("acompanhamento_aluno_foto");
            Map(a => a.AcompanhamentoAlunoSemestreId).ToColumn("acompanhamento_aluno_semestre_id");
            Map(a => a.ArquivoId).ToColumn("arquivo_id");
            Map(a => a.MiniaturaId).ToColumn("miniatura_id");
        }
    }
}
