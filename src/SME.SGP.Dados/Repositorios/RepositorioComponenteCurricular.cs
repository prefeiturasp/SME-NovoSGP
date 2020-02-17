using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioComponenteCurricular : RepositorioBase<ComponenteCurricular>, IRepositorioComponenteCurricular
    {
        public RepositorioComponenteCurricular(ISgpContext conexao) : base(conexao)
        {
        }

        public IEnumerable<ComponenteCurricular> ObterComponentesJuremaPorCodigoEol(long codigoEol)
        {
            var query = "select * from componente_curricular where codigo_eol = @codigoEol";
            return database.Conexao.Query<ComponenteCurricular>(query, new { codigoEol = codigoEol.ToString() });
        }
    }
}