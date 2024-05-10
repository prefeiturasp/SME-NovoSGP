using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;

namespace SME.SGP.Dados
{
    public class RepositorioPerfilEventoTipo : RepositorioBase<PerfilEventoTipo>, IRepositorioPerfilEventoTipo
    {
        public RepositorioPerfilEventoTipo(ISgpContext context, IServicoAuditoria servicoAuditoria) : base(context, servicoAuditoria)
        {
        }
    }
}
