using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados
{
    public class RepositorioOcorrenciaTipo : RepositorioBase<OcorrenciaTipo>, IRepositorioOcorrenciaTipo
    {
        public RepositorioOcorrenciaTipo(ISgpContext conexao) : base(conexao) { }
    }
}
