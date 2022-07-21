using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioOpcaoQuestaoComplementar : RepositorioBase<OpcaoQuestaoComplementar>, IRepositorioOpcaoQuestaoComplementar
    {
        public RepositorioOpcaoQuestaoComplementar(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }
    }
}
