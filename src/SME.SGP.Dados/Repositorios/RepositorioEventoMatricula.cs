using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioEventoMatricula: RepositorioBase<EventoMatricula>, IRepositorioEventoMatricula
    {
        public RepositorioEventoMatricula(ISgpContext conexao) : base(conexao)
        {
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
