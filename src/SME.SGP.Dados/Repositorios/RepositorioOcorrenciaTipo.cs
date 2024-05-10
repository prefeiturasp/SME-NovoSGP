using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;

namespace SME.SGP.Dados
{
    public class RepositorioOcorrenciaTipo : RepositorioBase<OcorrenciaTipo>, IRepositorioOcorrenciaTipo
    {
        public RepositorioOcorrenciaTipo(ISgpContext conexao, IServicoAuditoria servicoAuditoria) : base(conexao, servicoAuditoria) { }
    }
}
