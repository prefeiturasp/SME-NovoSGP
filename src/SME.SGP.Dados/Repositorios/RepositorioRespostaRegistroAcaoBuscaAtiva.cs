using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioRespostaRegistroAcaoBuscaAtiva : RepositorioBase<RespostaRegistroAcaoBuscaAtiva>, IRepositorioRespostaRegistroAcaoBuscaAtiva
    {
        public RepositorioRespostaRegistroAcaoBuscaAtiva(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public Task<IEnumerable<long>> ObterArquivosPorQuestaoId(long questaoRegistroAcaoId)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<RespostaRegistroAcaoBuscaAtiva>> ObterPorQuestaoRegistroAcaoId(long questaoRegistroAcaoId)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> RemoverPorArquivoId(long arquivoId)
        {
            throw new System.NotImplementedException();
        }
    }
}
