using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAlunoFoto : RepositorioBase<AlunoFoto>, IRepositorioAlunoFoto
    {
        public RepositorioAlunoFoto(ISgpContext conexao) : base(conexao)
        {
        }
    }
}
