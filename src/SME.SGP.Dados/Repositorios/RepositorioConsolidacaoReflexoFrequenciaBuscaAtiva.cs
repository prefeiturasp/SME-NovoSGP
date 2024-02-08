using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioConsolidacaoReflexoFrequenciaBuscaAtiva : RepositorioBase<ConsolidadoEncaminhamentoNAAPA>, IRepositorioConsolidacaoReflexoFrequenciaBuscaAtiva
    {
        public RepositorioConsolidacaoReflexoFrequenciaBuscaAtiva(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }
    }
}