using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAnotacaoFrequenciaAluno : RepositorioBase<AnotacaoFrequenciaAluno>, IRepositorioAnotacaoFrequenciaAluno
    {
        public RepositorioAnotacaoFrequenciaAluno(ISgpContext conexao) : base(conexao)
        {
        }
    }
}