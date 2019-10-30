using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioEvento : RepositorioBase<Evento>, IRepositorioEvento
    {
        public RepositorioEvento(ISgpContext conexao) : base(conexao)
        {
        }

        public bool ExisteEventoNaMesmaDataECalendario(DateTime dataInicio, long tipoCalendarioId)
        {
            var query = "select 1 from evento where data_inicio = @dataInicio and tipo_calendario = @tipoCalendarioId;";
            return database.Conexao.QueryFirstOrDefault<bool>(query, new { dataInicio, tipoCalendarioId });
        }
    }
}