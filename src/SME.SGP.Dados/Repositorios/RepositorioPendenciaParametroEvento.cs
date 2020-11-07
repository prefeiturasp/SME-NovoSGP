using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados
{
    public class RepositorioPendenciaParametroEvento : RepositorioBase<PendenciaParametroEvento>, IRepositorioPendenciaParametroEvento
    {
        public RepositorioPendenciaParametroEvento(ISgpContext database) : base(database)
        {
        }
    }
}
