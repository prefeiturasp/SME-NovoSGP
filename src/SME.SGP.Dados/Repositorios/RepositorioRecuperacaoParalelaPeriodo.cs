using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioRecuperacaoParalelaPeriodo : RepositorioBase<RecuperacaoParalelaPeriodo>, IRepositorioRecuperacaoParalelaPeriodo
    {
        public RepositorioRecuperacaoParalelaPeriodo(ISgpContext conexao) : base(conexao)
        {
        }
    }
}