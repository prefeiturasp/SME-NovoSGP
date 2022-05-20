using SME.SGP.Dominio;

namespace SME.SGP.Dados
{ 
    public class EncaminhamentoAEEMap : BaseMap<EncaminhamentoAEE>
    {
        public EncaminhamentoAEEMap()
        {
            ToTable("encaminhamento_aee");

            Map(c => c.TurmaId).ToColumn("turma_id");
            Map(c => c.AlunoCodigo).ToColumn("aluno_codigo");
            Map(c => c.AlunoNome).ToColumn("aluno_nome");
            Map(c => c.Situacao).ToColumn("situacao");
            Map(c => c.Excluido).ToColumn("excluido");
            Map(c => c.MotivoEncerramento).ToColumn("motivo_encerramento");
            Map(c => c.ResponsavelId).ToColumn("responsavel_id");
        }
    }
}
