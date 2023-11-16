using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioRespostaRegistroAcaoBuscaAtiva : RepositorioBase<RespostaRegistroAcaoBuscaAtiva>, IRepositorioRespostaRegistroAcaoBuscaAtiva
    {
        public RepositorioRespostaRegistroAcaoBuscaAtiva(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }
    }
}
