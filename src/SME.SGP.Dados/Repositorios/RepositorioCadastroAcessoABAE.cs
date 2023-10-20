using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioCadastroAcessoABAE : RepositorioBase<CadastroAcessoABAE>, IRepositorioCadastroAcessoABAE
    {
        public RepositorioCadastroAcessoABAE(ISgpContext conexao, IServicoAuditoria servicoAuditoria) : base(conexao, servicoAuditoria)
        {}
    }
}