using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioRecuperacaoParalelaPeriodo : RepositorioBase<RecuperacaoParalelaPeriodo>, IRepositorioRecuperacaoParalelaPeriodo
    {
        public RepositorioRecuperacaoParalelaPeriodo(ISgpContext conexao, IServicoAuditoria servicoAuditoria) : base(conexao, servicoAuditoria)
        {
        }
    }
}