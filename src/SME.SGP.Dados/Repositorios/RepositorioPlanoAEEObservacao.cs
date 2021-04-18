using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados
{
    public class RepositorioPlanoAEEObservacao : RepositorioBase<PlanoAEEObservacao>, IRepositorioPlanoAEEObservacao
    {
        public RepositorioPlanoAEEObservacao(ISgpContext database) : base(database)
        {
        }
    }
}
