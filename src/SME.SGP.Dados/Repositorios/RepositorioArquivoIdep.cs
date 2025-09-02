using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioArquivoIdep : RepositorioBase<Idep>, IRepositorioArquivoIdep
    {
        public RepositorioArquivoIdep(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }
    }
}
