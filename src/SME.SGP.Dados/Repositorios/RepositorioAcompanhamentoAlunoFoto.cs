using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAcompanhamentoAlunoFoto : RepositorioBase<AcompanhamentoAlunoFoto>, IRepositorioAcompanhamentoAlunoFoto
    {
        public RepositorioAcompanhamentoAlunoFoto(ISgpContext conexao) : base(conexao)
        {
        }
    }
}
