using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Dados
{
    public class RepositorioOcorrenciaAluno : RepositorioBase<OcorrenciaAluno>, IRepositorioOcorrenciaAluno
    {
        public RepositorioOcorrenciaAluno(ISgpContext conexao) : base(conexao) { }
    }
}
