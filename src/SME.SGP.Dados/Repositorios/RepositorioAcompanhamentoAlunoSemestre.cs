using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAcompanhamentoAlunoSemestre : RepositorioBase<AcompanhamentoAlunoSemestre>, IRepositorioAcompanhamentoAlunoSemestre
    {
        public RepositorioAcompanhamentoAlunoSemestre(ISgpContext conexao) : base(conexao)
        {
        }
    }
}
