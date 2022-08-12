using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioFechamentoNota : RepositorioBase<FechamentoNota>, IRepositorioFechamentoNota
    {
        public RepositorioFechamentoNota(ISgpContext database) : base(database)
        {
        }
    }
}