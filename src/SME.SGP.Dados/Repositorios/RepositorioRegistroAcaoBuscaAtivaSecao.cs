using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioRegistroAcaoBuscaAtivaSecao : RepositorioBase<RegistroAcaoBuscaAtivaSecao>, IRepositorioRegistroAcaoBuscaAtivaSecao
    {
        public RepositorioRegistroAcaoBuscaAtivaSecao(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public Task<AuditoriaDto> ObterAuditoriaRegistroAcaoSecao(long id)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<long>> ObterIdsSecoesPorRegistroAcaoId(long registroAcaoId)
        {
            throw new System.NotImplementedException();
        }
    }
}
