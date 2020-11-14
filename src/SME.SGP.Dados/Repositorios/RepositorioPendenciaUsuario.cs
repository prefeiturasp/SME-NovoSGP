using Npgsql;
using NpgsqlTypes;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioPendenciaUsuario : RepositorioBase<PendenciaUsuario>, IRepositorioPendenciaUsuario
    {
        public RepositorioPendenciaUsuario(ISgpContext database) : base(database)
        {
        }        
    }
}
