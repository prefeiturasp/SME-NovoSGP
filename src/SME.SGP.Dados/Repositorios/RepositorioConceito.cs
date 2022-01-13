using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioConceito : RepositorioBase<Conceito>, IRepositorioConceito
    {
        public RepositorioConceito(ISgpContext database) : base(database)
        {
        }        
    }
}