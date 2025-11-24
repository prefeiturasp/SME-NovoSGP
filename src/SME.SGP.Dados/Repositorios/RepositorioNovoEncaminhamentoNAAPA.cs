using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioNovoEncaminhamentoNAAPA : RepositorioBase<EncaminhamentoNAAPA>, IRepositorioNovoEncaminhamentoNAAPA
    {
        public RepositorioNovoEncaminhamentoNAAPA(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

    }
}