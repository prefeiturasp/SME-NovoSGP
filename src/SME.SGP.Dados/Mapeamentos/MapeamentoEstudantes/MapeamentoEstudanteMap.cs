using SME.SGP.Dominio;
namespace SME.SGP.Dados
{ 
    public class MapeamentoEstudanteMap : BaseMap<MapeamentoEstudante>
    {
        public MapeamentoEstudanteMap()
        {
            ToTable("mapeamento_estudante");
            Map(c => c.TurmaId).ToColumn("turma_id");
            Map(c => c.AlunoCodigo).ToColumn("aluno_codigo");
            Map(c => c.AlunoNome).ToColumn("aluno_nome");
            Map(c => c.Bimestre).ToColumn("bimestre");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}
