using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioFrequenciaAlunoDisciplinaPeriodo : RepositorioBase<FrequenciaAluno>, IRepositorioFrequenciaAlunoDisciplinaPeriodo
    {
        public RepositorioFrequenciaAlunoDisciplinaPeriodo(ISgpContext database) : base(database)
        {
        }

        public FrequenciaAluno Obter(string codigoAluno, string disciplinaId, DateTime periodoInicio, DateTime periodoFim, TipoFrequenciaAluno tipoFrequencia)
        {
            var query = @"select
	                        *
                        from
	                        frequencia_aluno
                        where
	                        codigo_aluno = @codigoAluno
	                        and disciplina_id = @disciplinaId
	                        and tipo = @tipoFrequencia
	                        and periodo_inicio = @periodoInicio
	                        and periodo_fim = @periodoFim";
            return database.QueryFirstOrDefault<FrequenciaAluno>(query, new
            {
                codigoAluno,
                disciplinaId,
                periodoInicio,
                periodoFim,
                tipoFrequencia
            });
        }

        public IEnumerable<FrequenciaAluno> ObterAlunosComAusenciaPorDisciplinaNoPeriodo(long periodoId)
        {
            var query = @"select f.* 
                          from frequencia_aluno f
                         inner join periodo_escolar p on p.periodo_fim = f.periodo_fim and p.periodo_inicio = f.periodo_inicio
                        where not f.excluido
                          and p.id = @periodoId
                          and f.tipo = 1
                          and f.total_ausencias - f.total_compensacoes > 0 ";

            return database.Conexao.Query<FrequenciaAluno>(query, new { periodoId });
        }

        public FrequenciaAluno ObterPorAlunoData(string codigoAluno, DateTime dataAtual, TipoFrequenciaAluno tipoFrequencia, string disciplinaId = "")
        {
            var query = new StringBuilder(@"select *
                        from frequencia_aluno
                        where
	                        codigo_aluno = @codigoAluno
	                        and tipo = @tipoFrequencia
	                        and periodo_inicio <= @dataAtual
	                        and periodo_fim >= @dataAtual ");

            if (!string.IsNullOrEmpty(disciplinaId))
                query.AppendLine("and disciplina_id = @disciplinaId");

            return database.QueryFirstOrDefault<FrequenciaAluno>(query.ToString(), new
            {
                codigoAluno,
                dataAtual,
                tipoFrequencia,
                disciplinaId
            });
        }
        
        public FrequenciaAluno ObterPorAlunoDisciplinaData(string codigoAluno, string disciplinaId, DateTime dataAtual)
        {
            var query = @"select *
                        from frequencia_aluno
                        where codigo_aluno = @codigoAluno
                            and disciplina_id = @disciplinaId
	                        and tipo = 1
	                        and periodo_inicio <= @dataAtual
	                        and periodo_fim >= @dataAtual";

            return database.QueryFirstOrDefault<FrequenciaAluno>(query, new
            {
                codigoAluno,
                disciplinaId,
                dataAtual,
            });
        }


    }
}