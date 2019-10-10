using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dto;
using System.Collections.Generic;
using Dapper;
using System.Text;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioTipoCalendarioEscolar : RepositorioBase<TipoCalendarioEscolar>, IRepositorioTipoCalendarioEscolar
    {
        public RepositorioTipoCalendarioEscolar(ISgpContext conexao) : base(conexao)
        {
        }

        public IEnumerable<TipoCalendarioEscolarDto> ObterTiposCalendarioEscolar()
        {
            StringBuilder query = new StringBuilder();

            query.AppendLine("select ");
            query.AppendLine("id, ");
            query.AppendLine("nome, ");
            query.AppendLine("ano_letivo, ");
            query.AppendLine("periodo ");
            query.AppendLine("from tipo_calendario_escolar");

            return database.Conexao.Query<TipoCalendarioEscolarDto>(query.ToString());
        }
    }
}
