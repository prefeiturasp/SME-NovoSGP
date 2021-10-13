using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados
{
    public class RepositorioConsolidacaoRegistrosPedagogicos : IRepositorioConsolidacaoRegistrosPedagogicos
    {
        private readonly ISgpContext database;

        public RepositorioConsolidacaoRegistrosPedagogicos(ISgpContext database)
        {
            this.database = database ?? throw new System.ArgumentNullException(nameof(database));
        }
    }
}
