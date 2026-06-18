using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;

namespace SME.SGP.Dados
{
    public class RepositorioHistoricoReinicioSenha : RepositorioBase<HistoricoReinicioSenha>, IRepositorioHistoricoReinicioSenha
    {
        public RepositorioHistoricoReinicioSenha(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }
    }
}
