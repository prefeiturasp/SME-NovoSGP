using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAcompanhamentoAluno : RepositorioBase<AcompanhamentoAluno>, IRepositorioAcompanhamentoAluno
    {
        public RepositorioAcompanhamentoAluno(ISgpContext conexao) : base(conexao)
        {
        }
    }
}
