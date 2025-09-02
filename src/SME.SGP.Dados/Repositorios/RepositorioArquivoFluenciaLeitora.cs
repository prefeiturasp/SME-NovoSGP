using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioArquivoFluenciaLeitora : RepositorioBase<FluenciaLeitora>, IRepositorioArquivoFluenciaLeitora
    {
        public RepositorioArquivoFluenciaLeitora(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }
    }
}
