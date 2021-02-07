using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPlanoAEE : RepositorioBase<PlanoAEE>, IRepositorioPlanoAEE
    {
        public RepositorioPlanoAEE(ISgpContext database) : base(database)
        {
        }
    }
}
