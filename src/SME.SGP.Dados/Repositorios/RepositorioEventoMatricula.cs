using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioEventoMatricula: RepositorioBase<EventoMatricula>, IRepositorioEventoMatricula
    {
        public RepositorioEventoMatricula(ISgpContext conexao) : base(conexao)
        {
        }

        public EventoMatricula ObterUltimoEventoAluno(string codigoAluno, DateTime dataLimite)
        {
            var query = @"select * 
                          from evento_matricula 
                         where codigo_aluno = @codigoAluno
                           and data_evento <= @dataLimite
                        order by data_evento desc";

            return database.Conexao.QueryFirstOrDefault<EventoMatricula>(query, new { codigoAluno, dataLimite });
        }

        public bool CheckarEventoExistente(SituacaoMatriculaAluno tipo, DateTime dataEvento, string codigoAluno)
        {
            var query = @"select 1 
                          from evento_matricula
                         where tipo = @tipo
                           and data_evento = @dataEvento
                           and codigo_aluno = @codigoAluno";

            return database.Conexao.Query<bool>(query, new { tipo, dataEvento, codigoAluno }).SingleOrDefault();
        }
    }
}
