using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioComponenteCurricularJurema : RepositorioBase<ComponenteCurricularJurema>, IRepositorioComponenteCurricularJurema
    {
        public RepositorioComponenteCurricularJurema(ISgpContext conexao) : base(conexao)
        {
        }

        public IEnumerable<ComponenteCurricularJurema> ObterComponentesJuremaPorCodigoEol(long codigoEol)
        {
            var query = "select * from componente_curricular_jurema where codigo_eol = @codigoEol";
            return database.Conexao.Query<ComponenteCurricularJurema>(query, new { codigoEol });
        }
    }
}