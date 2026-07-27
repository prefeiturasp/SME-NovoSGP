using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class AcompanhamentoAlunoFotoMap : BaseEntityMap<AcompanhamentoAlunoFoto>
    {
        public AcompanhamentoAlunoFotoMap()
        {
            ToTable("acompanhamento_aluno_foto");
            Map(nameof(AcompanhamentoAlunoFoto.AcompanhamentoAlunoSemestreId), "acompanhamento_aluno_semestre_id");
            Map(nameof(AcompanhamentoAlunoFoto.ArquivoId), "arquivo_id");
            Map(nameof(AcompanhamentoAlunoFoto.MiniaturaId), "miniatura_id");
            Map(nameof(AcompanhamentoAlunoFoto.Excluido), "excluido");
        }
    }
}
