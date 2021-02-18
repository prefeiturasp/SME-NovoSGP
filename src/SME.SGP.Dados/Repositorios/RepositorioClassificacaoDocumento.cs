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
    public class RepositorioClassificacaoDocumento : IRepositorioClassificacaoDocumento
    {
        protected readonly ISgpContext database;

        public RepositorioClassificacaoDocumento(ISgpContext database)
        {
            this.database = database;
        }

        public async Task<bool> ValidarTipoDocumento(long classificacaoDocumentoId, int tipoDocumento)
        {
            var query = "select 1 from classificacao_documento where id = @classificacaoDocumentoId and tipo_documento_id = @tipoDocumento";

            return await database.Conexao.QueryFirstOrDefaultAsync<bool>(query, new { classificacaoDocumentoId, tipoDocumento });
        }
    }
}