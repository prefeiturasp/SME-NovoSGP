using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados
{
    public class RepositorioPendenciaPerfil : RepositorioBase<PendenciaPerfil>, IRepositorioPendenciaPerfil
    {
        public RepositorioPendenciaPerfil(ISgpContext database) : base(database)
        {
        }
    }
}
