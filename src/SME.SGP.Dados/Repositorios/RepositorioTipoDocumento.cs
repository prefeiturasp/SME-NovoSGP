using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioTipoDocumento : IRepositorioTipoDocumento
    {
        protected readonly ISgpContext database;

        public RepositorioTipoDocumento(ISgpContext database)
        {
            this.database = database;
        }

    }
}