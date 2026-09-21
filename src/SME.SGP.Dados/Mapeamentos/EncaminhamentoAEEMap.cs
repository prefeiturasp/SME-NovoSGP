using SME.SGP.Dados.Mapeamentos;
using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class EncaminhamentoAEEMap : BaseMap<EncaminhamentoAEE>
    {
        public EncaminhamentoAEEMap()
        {
            ToTable("encaminhamento_aee");
            Map(nameof(EncaminhamentoAEE.TurmaId), "turma_id");
            Map(nameof(EncaminhamentoAEE.AlunoCodigo), "aluno_codigo");
            Map(nameof(EncaminhamentoAEE.AlunoNome), "aluno_nome");
            Map(nameof(EncaminhamentoAEE.Situacao), "situacao");
            Map(nameof(EncaminhamentoAEE.Excluido), "excluido");
            Map(nameof(EncaminhamentoAEE.MotivoEncerramento), "motivo_encerramento");
            Map(nameof(EncaminhamentoAEE.ResponsavelId), "responsavel_id");
        }
    }
}