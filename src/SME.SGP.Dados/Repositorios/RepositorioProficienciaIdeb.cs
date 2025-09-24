using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioProficienciaIdeb : RepositorioBase<Dominio.Entidades.ProficienciaIdeb>, IRepositorioProficienciaIdeb
    {
        public RepositorioProficienciaIdeb(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }
    }
}
