using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioRegistroAcaoBuscaAtiva : RepositorioBase<RegistroAcaoBuscaAtiva>, IRepositorioRegistroAcaoBuscaAtiva
    {
        public RepositorioRegistroAcaoBuscaAtiva(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public Task<RegistroAcaoBuscaAtiva> ObterRegistroAcaoCabecalhoPorId(long id)
        {
            throw new System.NotImplementedException();
        }

        public Task<RegistroAcaoBuscaAtiva> ObterRegistroAcaoComTurmaPorId(long id)
        {
            throw new System.NotImplementedException();
        }

        public Task<RegistroAcaoBuscaAtiva> ObterRegistroAcaoPorId(long id)
        {
            throw new System.NotImplementedException();
        }

        public Task<RegistroAcaoBuscaAtiva> ObterRegistroAcaoPorIdESecao(long id, long secaoId)
        {
            throw new System.NotImplementedException();
        }
    }
}
