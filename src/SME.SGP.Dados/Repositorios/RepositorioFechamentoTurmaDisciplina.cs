using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioFechamentoTurmaDisciplina : RepositorioBase<FechamentoTurmaDisciplina>, IRepositorioFechamentoTurmaDisciplina
    {
        public RepositorioFechamentoTurmaDisciplina(ISgpContext database) : base(database)
        {
        }

    }
}