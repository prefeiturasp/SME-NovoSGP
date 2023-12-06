using SME.SGP.Dominio;

namespace SME.SGP.Dados
{ 
    public class RegistroAcaoBuscaAtivaMap : BaseMap<RegistroAcaoBuscaAtiva>
    {
        public RegistroAcaoBuscaAtivaMap()
        {
            ToTable("registro_acao_busca_ativa");
            Map(c => c.TurmaId).ToColumn("turma_id");
            Map(c => c.AlunoCodigo).ToColumn("aluno_codigo");
            Map(c => c.AlunoNome).ToColumn("aluno_nome");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}
