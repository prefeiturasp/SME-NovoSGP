using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPeriodoEscolar : RepositorioBase<PeriodoEscolar>, IRepositorioPeriodoEscolar
    {
        public RepositorioPeriodoEscolar(ISgpContext conexao) : base(conexao)
        {
        }

        public IList<PeriodoEscolar> ObterPorTipoCalendario(long codigoTipoCalendario)
        {
            StringBuilder query = new StringBuilder();

            query.AppendLine("select * from periodo_escolar");

            query.AppendLine("where tipo_calendario_id = @tipoCalendario");

            return database.Conexao.Query<PeriodoEscolar>(query.ToString(), new {tipoCalendario = codigoTipoCalendario }).ToList();
        }
    }
}
