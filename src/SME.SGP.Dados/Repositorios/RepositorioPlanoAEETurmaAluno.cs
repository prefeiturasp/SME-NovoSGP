using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;

namespace SME.SGP.Dados
{
    public class RepositorioPlanoAEETurmaAluno : RepositorioBase<PlanoAEETurmaAluno>, IRepositorioPlanoAEETurmaAluno
    {
        public RepositorioPlanoAEETurmaAluno(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }
    }
}
