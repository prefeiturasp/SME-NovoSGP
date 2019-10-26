using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Linq;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioEvento : RepositorioBase<Evento>, IRepositorioEvento
    {
        public RepositorioEvento(ISgpContext conexao) : base(conexao)
        {
        }

        public bool ExisteEventoNaDataEspecificada(DateTime dataInicio)
        {
            return database.Conexao.Query<bool>("select 1 from evento where data_inicio = @dataInicio;", new { dataInicio }).FirstOrDefault();
        }
    }
}