using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class RegistroAcaoBuscaAtivaMap : BaseEntityMap<RegistroAcaoBuscaAtiva>
    {
        public RegistroAcaoBuscaAtivaMap()
        {
            ToTable("registro_acao_busca_ativa");
            Map(nameof(RegistroAcaoBuscaAtiva.TurmaId), "turma_id");
            Map(nameof(RegistroAcaoBuscaAtiva.AlunoCodigo), "aluno_codigo");
            Map(nameof(RegistroAcaoBuscaAtiva.AlunoNome), "aluno_nome");
            Map(nameof(RegistroAcaoBuscaAtiva.Excluido), "excluido");
        }
    }
}