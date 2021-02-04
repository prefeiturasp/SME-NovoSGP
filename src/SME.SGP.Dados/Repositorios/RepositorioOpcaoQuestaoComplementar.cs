using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioOpcaoQuestaoComplementar : RepositorioBase<OpcaoQuestaoComplementar>, IRepositorioOpcaoQuestaoComplementar
    {
        public RepositorioOpcaoQuestaoComplementar(ISgpContext database) : base(database)
        {
        }
    }
}
