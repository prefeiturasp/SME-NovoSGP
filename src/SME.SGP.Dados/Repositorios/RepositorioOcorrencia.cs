using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra;

namespace SME.SGP.Dados
{
    public class RepositorioOcorrencia : RepositorioBase<Ocorrencia>, IRepositorioOcorrencia
    {
        public RepositorioOcorrencia(ISgpContext conexao) : base(conexao) { }
    }
}
