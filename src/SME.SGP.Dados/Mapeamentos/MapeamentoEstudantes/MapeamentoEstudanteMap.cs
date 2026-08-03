using SME.SGP.Dados.Mapeamentos;
using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class MapeamentoEstudanteMap : BaseMap<MapeamentoEstudante>
    {
        public MapeamentoEstudanteMap()
        {
            ToTable("mapeamento_estudante");

            Map(nameof(MapeamentoEstudante.TurmaId), "turma_id");
            Map(nameof(MapeamentoEstudante.AlunoCodigo), "aluno_codigo");
            Map(nameof(MapeamentoEstudante.AlunoNome), "aluno_nome");
            Map(nameof(MapeamentoEstudante.Bimestre), "bimestre");
            Map(nameof(MapeamentoEstudante.Excluido), "excluido");
        }
    }
}