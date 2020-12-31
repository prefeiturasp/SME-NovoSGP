using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioSecaoEncaminhamentoAEE : RepositorioBase<SecaoEncaminhamentoAEE>, IRepositorioSecaoEncaminhamentoAEE
    {
        public RepositorioSecaoEncaminhamentoAEE(ISgpContext database) : base(database)
        {
        }
    }
}
