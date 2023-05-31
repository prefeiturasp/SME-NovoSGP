using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAnotacaoFechamentoAluno : RepositorioBase<AnotacaoFechamentoAluno>, IRepositorioAnotacaoFechamentoAluno
    {
        public RepositorioAnotacaoFechamentoAluno(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {}
    }
}
