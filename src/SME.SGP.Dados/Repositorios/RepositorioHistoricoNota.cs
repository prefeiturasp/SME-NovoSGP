using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioHistoricoNota : RepositorioBase<HistoricoNota>, IRepositorioHistoricoNota
    {
        public RepositorioHistoricoNota(ISgpContext conexao) : base(conexao)
        {

        }
    }
}
